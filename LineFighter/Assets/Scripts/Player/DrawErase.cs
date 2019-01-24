using Assets.Scripts.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class DrawErase : NetworkBehaviour
{
    #region Member Variables
    private Camera _cameraMain;
    private HudController _hudController;
    private PlayerController _playerController;
    private GameObject _playerLines;
    private EdgeCollider2D _lineCollider;
    private GameObject _lineObject;
    private Vector3 _mousePos;
    private LineRenderer _renderer;
    private bool _isDrawing = false;
    private List<SerializableVector2> _pointsList;

    private NetworkController _networkController;
    private bool _sendNetworkLineSpawn = false;
    private bool _sendNetworkLineUpdate = false;
    private float _timeSinceLastUpdate = 0f;

    #endregion Member Variables

    #region Properties..
    public Player Player { get; set; } 
    #endregion Properties..

    #region Events..
    private void Start()
    {
        if (Player.IsLocalPlayer)
        {
            Initialize();
        }
        else
        {
            this.enabled = false;
        }
    }

    void Update()
    {
        if (_hudController == null)
        {
            _hudController = GameObject.FindObjectOfType<HudController>();
        }

        switch (_hudController.DrawMode)
        {
            case HudController.DrawType.Draw:
                DrawLine();
                break;
            case HudController.DrawType.Erase:
                EraseLine();
                break;
        }

        // Enforce a limit on network requests of 10/second
        if (_timeSinceLastUpdate > 0.1f)
        {
            if (_sendNetworkLineSpawn)
            {
                _networkController.ClientSpawnPlayerLine(_lineObject);

                _timeSinceLastUpdate = 0f;
                _sendNetworkLineSpawn = false;
            }
            else if (_sendNetworkLineUpdate)
            {
                _networkController.ClientUpdatePlayerLine(_lineObject, _pointsList);

                _timeSinceLastUpdate = 0f;
                _sendNetworkLineUpdate = false;
            }
        }
        else
        {
            _timeSinceLastUpdate += Time.deltaTime;
        }
    }
    #endregion Events..

    #region Public Methods
    public void Initialize()
    {
        _cameraMain = Camera.main;
        _hudController = GameObject.FindObjectOfType<HudController>();
        _networkController = GameObject.FindObjectOfType<NetworkController>();
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _playerLines = gameObject.GetComponentInParent<Transform>().gameObject;

        _pointsList = new List<SerializableVector2>();
    }

    public void SetLineProperties(LineRenderer lineRenderer, Line line)
    {
        lineRenderer.material = AssetLibrary.MaterialAssets[Fields.Assets.LineMaterialBase];
        lineRenderer.startColor = new Color(255, 255, 255, 100);
        lineRenderer.endColor = new Color(255, 255, 255, 100);
        lineRenderer.startWidth = (float)line.Thickness;
        lineRenderer.endWidth = (float)line.Thickness;
        lineRenderer.useWorldSpace = true;
    }
    #endregion Public Methods

    #region Private Methods
    private void DrawLine()
    {
        try
        {
            if (Input.GetMouseButtonDown(1))
            {
                _pointsList.Clear();
                _isDrawing = true;

                // Create a new line Object
                _lineObject = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.LineObjectPrefab]);
                 _lineObject.transform.parent = _playerLines.transform;
                _renderer = _lineObject.GetComponent<LineRenderer>();
                SetLineProperties(_renderer, _playerController.Line);

                _sendNetworkLineSpawn = true;
            }

            // Drawing line when mouse is moving(presses)
            if (Input.GetMouseButton(1) && _isDrawing)
            {
                _mousePos = _cameraMain.ScreenPointToRay(Input.mousePosition).origin;
                _mousePos.z = 0;
                if (!_pointsList.Contains((Vector2)_mousePos))
                {
                    _pointsList.Add(new Vector2(_mousePos.x, _mousePos.y));
                    _renderer.positionCount = _pointsList.Count;
                    _renderer.SetPosition(_pointsList.Count - 1, (Vector2)_pointsList[_pointsList.Count - 1]);

                    _sendNetworkLineUpdate = true;
                }
            }

            // Finish the line
            if (!Input.GetMouseButton(1) && _isDrawing)
            {
                _isDrawing = false;

                // Collider
                if (_pointsList.Count > 1)
                {
                    _lineCollider = _lineObject.GetComponent<EdgeCollider2D>();
                    _lineCollider.edgeRadius = (float)_playerController.Line.Thickness - 0.01f;
                    Vector2[] vertices = new Vector2[_pointsList.Count];

                    for (int i = 0; i < _pointsList.Count; i++)
                    {
                        vertices[i] = new Vector2(_pointsList[i].x, _pointsList[i].y);
                    }

                    _lineCollider.points = vertices;
                }
            }

            //NetworkServer.SpawnObjects();
        }
        catch { }
    }

    private void EraseLine()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isDrawing = true;
        }

        if (_isDrawing)
        {
            Ray mouseRay = _cameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] mouseHits = Physics2D.CircleCastAll(mouseRay.origin, _playerController.Eraser.Radius, mouseRay.direction);

            foreach (RaycastHit2D mouseHit in mouseHits)
            {
                try
                {
                    if (mouseHit && mouseHit.collider.tag == Fields.Tags.LineObject)
                    {
                        EdgeCollider2D lineCollider = (EdgeCollider2D) mouseHit.collider;
                        Vector2 contactPoint = mouseHit.point;
                        int closestPointIndex = -1;
                        float closestPointDistance = (float)int.MaxValue;

                        // Get the closest point to the collision
                        for (int i = 0; i < lineCollider.points.Length; i++)
                        {
                            float distance = Mathf.Sqrt(Mathf.Pow((lineCollider.points[i].x - contactPoint.x), 2) + Mathf.Pow((lineCollider.points[i].y - contactPoint.y), 2));
                            if (distance < closestPointDistance)
                            {
                                closestPointIndex = i;
                                closestPointDistance = distance;
                            }
                        }

                        // Split into two lines
                        int firstLineSize = closestPointIndex;
                        int secondLineSize = lineCollider.points.Length - closestPointIndex - 1;

                        Vector2[] firstLineV2Arr = new Vector2[firstLineSize];
                        Vector3[] firstLineV3Arr = new Vector3[firstLineSize];

                        Vector2[] secondLineV2Arr = new Vector2[secondLineSize];
                        Vector3[] secondLineV3Arr = new Vector3[secondLineSize];

                        for (int i = 0; i < lineCollider.points.Length; i++)
                        {
                            if (i < closestPointIndex)
                            {
                                firstLineV2Arr[i] = lineCollider.points[i];
                                firstLineV3Arr[i] = lineCollider.points[i];
                            }
                            else if (i > closestPointIndex)
                            {
                                secondLineV2Arr[i - closestPointIndex - 1] = lineCollider.points[i];
                                secondLineV3Arr[i - closestPointIndex - 1] = lineCollider.points[i];
                            }
                        }

                        // Re-reference the original line
                        DestroyImmediate(mouseHit.collider.gameObject);

                        if (firstLineV2Arr.Length > 1)
                        {
                            // Create a new GameObject, LineRenderer and Collider for the first new line
                            GameObject firstLineObject = (GameObject) Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.LineObjectPrefab]);
                            firstLineObject.transform.parent = _playerLines.transform;

                            LineRenderer lineRendererOne = firstLineObject.GetComponent<LineRenderer>();
                            SetLineProperties(lineRendererOne, _playerController.Line);

                            lineRendererOne.positionCount = firstLineV3Arr.Length;
                            lineRendererOne.SetPositions(firstLineV3Arr);

                            EdgeCollider2D lineColliderOne = firstLineObject.GetComponent<EdgeCollider2D>();
                            lineColliderOne.edgeRadius = (float)_playerController.Line.Thickness - 0.01f;
                            lineColliderOne.offset = new Vector2(0.0f, 0.00f);
                            Vector2[] lineOneVertices = new Vector2[firstLineV2Arr.Length];

                            for (int i = 0; i < firstLineV2Arr.Length; i++)
                            {
                                lineOneVertices[i] = new Vector2(firstLineV2Arr[i].x, firstLineV2Arr[i].y);
                            }

                            lineColliderOne.points = lineOneVertices;
                        }

                        // Create a new GameObject, LineRenderer and Collider for the second new line
                        if (secondLineV2Arr.Length > 1)
                        {
                            GameObject secondLineObject = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.LineObjectPrefab]);
                            secondLineObject.transform.parent = _playerLines.transform;

                            LineRenderer lineRendererTwo = secondLineObject.GetComponent<LineRenderer>();
                            SetLineProperties(lineRendererTwo, _playerController.Line);

                            lineRendererTwo.positionCount = secondLineV3Arr.Length;
                            lineRendererTwo.SetPositions(secondLineV3Arr);

                            EdgeCollider2D lineColliderTwo = secondLineObject.GetComponent<EdgeCollider2D>();
                            lineColliderTwo.edgeRadius = (float)_playerController.Line.Thickness - 0.01f;
                            lineColliderTwo.offset = new Vector2(0.0f, 0.04f);
                            Vector2[] lineTwoVertices = new Vector2[secondLineV2Arr.Length];

                            for (int i = 0; i < secondLineV2Arr.Length; i++)
                            {
                                lineTwoVertices[i] = new Vector2(secondLineV2Arr[i].x, secondLineV2Arr[i].y);
                            }

                            lineColliderTwo.points = lineTwoVertices;
                        }
                    }
                }
                catch { }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isDrawing = false;
        }
    }
    #endregion Private Methods
}

