using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawErase : MonoBehaviour
{
    #region Member Variables
    private Camera _cameraMain;
    private HudController _hudController;
    private Transform _parentObject;
    private PlayerController _playerController;
    private GameObject _playerLines;
    private EdgeCollider2D _lineCollider;
    private GameObject _lineObject;
    private Vector3 _mousePos;
    private LineRenderer _renderer;
    private bool _isMousePressed = false;
    private List<Vector2> _pointsList;
    #endregion Member Variables

    #region MonoBehaviour
    private void Start()
    {
        _cameraMain = Camera.main;
        _hudController = GameObject.FindObjectOfType<HudController>();
        _parentObject = gameObject.GetComponentInParent<Transform>();
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _playerLines = GameObject.Find(Fields.GameObjects.PlayerLines);
    }

    void Update()
    {
        switch (_hudController.DrawMode)
        {
            case HudController.DrawType.Draw:
                DrawLine();
                break;
            case HudController.DrawType.Erase:
                EraseLine();
                break;
        }
    }
    #endregion MonoBehaviour

    #region Private Methods
    private void DrawLine()
    {
        try
        {
            if (Input.GetMouseButtonDown(1))
            {
                _pointsList.Clear();
                _isMousePressed = true;

                // Create a new line Object
                _lineObject = new GameObject();
                _lineObject.name = Fields.GameObjects.LineObject;
                _lineObject.tag = Fields.Tags.LineObject;
                _lineObject.transform.parent = _parentObject;
                _lineObject.AddComponent<LineRenderer>();
                _renderer = _lineObject.GetComponent<LineRenderer>();
                SetLineProperties(_renderer);
            }

            // Drawing line when mouse is moving(presses)
            if (_isMousePressed)
            {
                _mousePos = _cameraMain.ScreenToWorldPoint(Input.mousePosition);
                _mousePos.z = 0;
                if (!_pointsList.Contains(_mousePos))
                {
                    _pointsList.Add(new Vector2(_mousePos.x, _mousePos.y));
                    _renderer.positionCount = _pointsList.Count;
                    _renderer.SetPosition(_pointsList.Count - 1, (Vector3)_pointsList[_pointsList.Count - 1]);
                }
            }

            // Finish the line
            if (Input.GetMouseButtonUp(1))
            {
                _isMousePressed = false;

                // Collider
                if (_pointsList.Count > 1)
                {
                    _lineCollider = _lineObject.AddComponent<EdgeCollider2D>();
                    _lineCollider.edgeRadius = _playerController.Line.Thickness - 0.01f;
                    _lineCollider.offset = new Vector2(0.0f, 0.04f);
                    Vector2[] vertices = new Vector2[_pointsList.Count];

                    for (int i = 0; i < _pointsList.Count; i++)
                    {
                        vertices[i] = new Vector2(_pointsList[i].x, _pointsList[i].y);
                    }

                    _lineCollider.points = vertices;
                }
            }
        }
        catch { }
    }

    private void EraseLine()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isMousePressed = true;
        }

        if (_isMousePressed)
        {
            Ray mouseRay = _cameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] mouseHits = Physics2D.CircleCastAll(mouseRay.origin, _playerController.Eraser.Radius, mouseRay.direction);

            foreach (RaycastHit2D mouseHit in mouseHits)
            {
                try
                {
                    if (mouseHit && (mouseHit.collider.tag == Fields.Tags.LineObject || mouseHit.collider.tag == Fields.Tags.SplitLine))
                    {
                        EdgeCollider2D lineCollider = (EdgeCollider2D)mouseHit.collider;
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
                            GameObject firstLineObject = new GameObject();
                            firstLineObject.name = Fields.GameObjects.LineObject;
                            firstLineObject.tag = Fields.Tags.SplitLine;
                            firstLineObject.transform.parent = _playerLines.transform;
                            firstLineObject.AddComponent<LineRenderer>();

                            LineRenderer lineRendererOne = firstLineObject.GetComponent<LineRenderer>();
                            SetLineProperties(lineRendererOne);

                            lineRendererOne.positionCount = firstLineV3Arr.Length;
                            lineRendererOne.SetPositions(firstLineV3Arr);

                            EdgeCollider2D lineColliderOne = firstLineObject.AddComponent<EdgeCollider2D>();
                            lineColliderOne.edgeRadius = _playerController.Line.Thickness - 0.01f;
                            lineColliderOne.offset = new Vector2(0.0f, 0.04f);
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
                            GameObject secondLineObject = new GameObject();
                            secondLineObject.name = Fields.GameObjects.LineObject;
                            secondLineObject.tag = Fields.Tags.SplitLine;
                            secondLineObject.transform.parent = _playerLines.transform;
                            secondLineObject.AddComponent<LineRenderer>();

                            LineRenderer lineRendererTwo = secondLineObject.GetComponent<LineRenderer>();
                            SetLineProperties(lineRendererTwo);

                            lineRendererTwo.positionCount = secondLineV3Arr.Length;
                            lineRendererTwo.SetPositions(secondLineV3Arr);

                            EdgeCollider2D lineColliderTwo = secondLineObject.AddComponent<EdgeCollider2D>();
                            lineColliderTwo.edgeRadius = _playerController.Line.Thickness - 0.01f;
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
            _isMousePressed = false;
        }
    }

    private void SetLineProperties(LineRenderer lineRenderer)
    {
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startColor = new Color(255, 255, 255, 100);
        lineRenderer.endColor = new Color(255, 255, 255, 100);
        lineRenderer.startWidth = _playerController.Line.Thickness;
        lineRenderer.endWidth = _playerController.Line.Thickness;
        lineRenderer.useWorldSpace = true;
    }
    #endregion Private Methods
}

