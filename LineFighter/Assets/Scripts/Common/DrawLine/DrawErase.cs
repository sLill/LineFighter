using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawErase : MonoBehaviour
{
    #region Member Variables..
    private const float MAX_ACCEPTABALE_COLLIDER_GAP = 2.0f;
    private List<Vector2> _listPoint = new List<Vector2>();
    private GameObject _currentLine;
    private GameObject _currentColliderObject;
    private GameObject _playerLines;
    private BoxCollider2D _currentBoxCollider2D;
    private LineRenderer _currentLineRenderer;
    private HudController _hudController;
    private ParticleSystem _currentLineFX;
    ParticleSystem.EmitParams _lineParticleParams = new ParticleSystem.EmitParams();
    private PlayerController _playerController;
    private bool _isDrawing;
    #endregion Member Variables..

    #region Properties..
    public Material LineMaterial { get; set; }

    public bool UseGravity { get; set; }

    public Color LineColor { get; set; }
    #endregion Properties..

    #region Events..
    #region MonoBehaviour
    private void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _playerLines = gameObject;
    }

    private void Update()
    {
        if (_hudController == null)
        {
            _hudController = GameObject.FindObjectOfType<HudController>();
        }

        switch (_hudController.DrawMode)
        {
            case DrawType.Draw:
                Draw();
                break;
            case DrawType.Erase:
                Erase();
                break;
        }
    }
    #endregion MonoBehaviour
    #endregion Events..

    #region Public Methods..
    public void ClearAll()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Line");

        foreach (var item in temp)
        {
            Destroy(item);
        }
    }

    public void SetLineProperties(LineRenderer lineRenderer, Line line)
    {
        LineColor = Color.white;
        LineMaterial = AssetLibrary.MaterialAssets["LineMaterial_Base"];
        UseGravity = line.UseGravity;

        lineRenderer.material = LineMaterial;
        //lineRenderer.material.EnableKeyword("_EMISSION");
        //lineRenderer.material.SetColor("_EmissionColor", this.LineColor);
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = (float)line.Thickness;
        lineRenderer.endWidth = (float)line.Thickness;
        lineRenderer.startColor = LineColor;
        lineRenderer.endColor = LineColor;
        lineRenderer.useWorldSpace = false;
    }

    public void SetRigidBodyProperties(Rigidbody2D rigidBody)
    {
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidBody.useAutoMass = false;
        rigidBody.mass = 1f;

        if (!UseGravity)
        {
            rigidBody.gravityScale = 0;
            rigidBody.AddForce(new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)), ForceMode2D.Impulse);
        }
    }

    #endregion Public Methods..

    #region Private Methods..
    private void Draw()
    {
        if (_playerController.Line.ResourceCurrent > 0.00f)
        {
            if (Input.GetMouseButton(1) && !_isDrawing)
            {
                _isDrawing = true;
                _listPoint.Clear();

                _currentLine = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Common.LineObjectPrefab]);
                _currentLine.transform.parent = GameObject.FindGameObjectWithTag(Fields.GameObjects.PlayerLines).transform;
                _currentLine.tag = Fields.Tags.LineObject;
                _currentLineFX = _currentLine.GetComponent<ParticleSystem>();
                _currentLineRenderer = _currentLine.GetComponent<LineRenderer>();

                SetLineProperties(_currentLineRenderer, _playerController.Line);
            }

            if (Input.GetMouseButton(1) && _isDrawing)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                _lineParticleParams.position = mousePos;
                _currentLineFX.Emit(_lineParticleParams, 10);

                if (!_listPoint.Contains(mousePos))
                {
                    _listPoint.Add(mousePos);
                    _currentLineRenderer.positionCount = _listPoint.Count;
                    _currentLineRenderer.SetPosition(_listPoint.Count - 1, _listPoint[_listPoint.Count - 1]);

                    if (_listPoint.Count >= 2)
                    {
                        Vector2 vector = _listPoint[_listPoint.Count - 2];
                        Vector2 vector2 = _listPoint[_listPoint.Count - 1];

                        _currentColliderObject = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Common.ColliderPrefab]);
                        _currentColliderObject.transform.position = ((vector + vector2) / 2f);
                        _currentColliderObject.transform.right = ((vector2 - vector).normalized);
                        _currentColliderObject.transform.parent = _currentLine.transform;
                        _currentBoxCollider2D = _currentColliderObject.GetComponent<BoxCollider2D>();
                        _currentBoxCollider2D.size = new Vector3((vector2 - vector).magnitude, (float)_playerController.Line.Thickness, (float)_playerController.Line.Thickness);
                        _currentBoxCollider2D.enabled = false;
                    }

                    // Calculate Draw resource used
                    if (_listPoint.Count > 1)
                    {
                        float distance = Vector2.Distance(_listPoint[_listPoint.Count - 1], _listPoint[_listPoint.Count - 2]);
                        _playerController.Line.ResourceCurrent -= (distance * 50);
                    }
                }
            }
        }

        if ((!Input.GetMouseButton(1) || _playerController.Line.ResourceCurrent <= 0) && _isDrawing)
        {
            if (_currentLine.transform.childCount > 0)
            {
                for (int j = 0; j < _currentLine.transform.childCount; j++)
                {
                    _currentLine.transform.GetChild(j).GetComponent<BoxCollider2D>().enabled = true;
                }

                Rigidbody2D rigidBody = _currentLine.GetComponent<Rigidbody2D>();
                SetRigidBodyProperties(rigidBody);
            }
            else
            {
                Destroy(_currentLine);
            }

            _isDrawing = false;
        }
    }

    private void Erase()
    {
        if (_playerController.Eraser.ResourceCurrent > 0.00f)
        {
            if (Input.GetMouseButton(1) && !_isDrawing)
            {
                _isDrawing = true;
            }

            if (Input.GetMouseButton(1) && _isDrawing)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D[] eraserHits = Physics2D.CircleCastAll(mouseRay.origin, _playerController.Eraser.Radius, Vector2.zero);

                try
                {
                    foreach (RaycastHit2D eraserHit in eraserHits)
                    {
                        if (eraserHit && eraserHit.collider.tag == Fields.Tags.LineObjectCollider)
                        {
                            BoxCollider2D lineCollider = (BoxCollider2D)eraserHit.collider;
                            Vector3 contactPoint = eraserHit.point;
                            int closestPointIndex = -1;
                            float closestPointDistance = (float)int.MaxValue;

                            LineRenderer renderer = lineCollider.GetComponentInParent<LineRenderer>();

                            // Get the closest point to the collision
                            for (int i = 0; i < renderer.positionCount; i++)
                            {
                                var pointPosition = renderer.gameObject.transform.TransformPoint(renderer.GetPosition(i));
                                var distance = (pointPosition.normalized - contactPoint.normalized).magnitude;

                                //float distance = Extensions.GetDistance(renderer.GetPosition(i), contactPoint);
                                if (distance < closestPointDistance)
                                {
                                    closestPointIndex = i;
                                    closestPointDistance = distance;
                                }
                            }

                            // Split into two lines
                            int firstLineSize = closestPointIndex;
                            int secondLineSize = renderer.positionCount - closestPointIndex - 1;

                            Vector3[] firstLineV3Arr = new Vector3[firstLineSize];
                            Vector3[] secondLineV3Arr = new Vector3[secondLineSize];

                            for (int i = 0; i < renderer.positionCount; i++)
                            {
                                if (i < closestPointIndex)
                                {
                                    firstLineV3Arr[i] = renderer.GetPosition(i);
                                }
                                else if (i > closestPointIndex)
                                {
                                    secondLineV3Arr[i - closestPointIndex - 1] = renderer.GetPosition(i);
                                }
                            }

                            Vector3 transformPosition = eraserHit.collider.gameObject.transform.parent.gameObject.transform.position;
                            Quaternion transformRotation = eraserHit.collider.gameObject.transform.parent.gameObject.transform.rotation;

                            // De-reference the original line
                            DestroyImmediate(eraserHit.collider.gameObject.transform.parent.gameObject);

                            if (firstLineV3Arr.Length > 1)
                            {
                                // Create a new GameObject, LineRenderer and Colliders for the first new line
                                GameObject firstLineObject = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Common.LineObjectPrefab]);
                                firstLineObject.transform.position = transformPosition;
                                firstLineObject.transform.parent = _playerLines.transform;

                                LineRenderer lineRendererOne = firstLineObject.GetComponent<LineRenderer>();
                                SetLineProperties(lineRendererOne, _playerController.Line);

                                lineRendererOne.positionCount = firstLineV3Arr.Length;
                                lineRendererOne.SetPositions(firstLineV3Arr);

                                // Colliders
                                if (firstLineV3Arr.Length >= 2)
                                {
                                    int endIndex = firstLineV3Arr.Length - (firstLineV3Arr.Length % 2);
                                    for (int i = 0; i < endIndex - 1; i++)
                                    {
                                        Vector2 vector = firstLineV3Arr[i];
                                        Vector2 vector2 = firstLineV3Arr[i + 1];

                                        GameObject currentColliderObject = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Common.ColliderPrefab]);
                                        currentColliderObject.transform.parent = firstLineObject.transform;
                                        currentColliderObject.transform.localPosition = ((vector + vector2) / 2);
                                        currentColliderObject.transform.right = ((vector2 - vector).normalized);
                                        BoxCollider2D currentBoxCollider2D = currentColliderObject.GetComponent<BoxCollider2D>();
                                        currentBoxCollider2D.size = new Vector3((vector2 - vector).magnitude, (float)_playerController.Line.Thickness, (float)_playerController.Line.Thickness);
                                    }
                                }

                                Rigidbody2D rigidBody = firstLineObject.GetComponent<Rigidbody2D>();
                                SetRigidBodyProperties(rigidBody);

                                firstLineObject.transform.rotation = transformRotation;
                            }

                            // Create a new GameObject, LineRenderer and Collider for the second new line
                            if (secondLineV3Arr.Length > 1)
                            {
                                GameObject secondLineObject = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Common.LineObjectPrefab]);
                                secondLineObject.transform.position = transformPosition;
                                secondLineObject.transform.parent = _playerLines.transform;

                                LineRenderer lineRendererTwo = secondLineObject.GetComponent<LineRenderer>();
                                SetLineProperties(lineRendererTwo, _playerController.Line);

                                lineRendererTwo.positionCount = secondLineV3Arr.Length;
                                lineRendererTwo.SetPositions(secondLineV3Arr);


                                // Colliders
                                if (secondLineV3Arr.Length >= 2)
                                {
                                    int endIndex = secondLineV3Arr.Length - (secondLineV3Arr.Length % 2);
                                    for (int i = 0; i < endIndex - 1; i++)
                                    {
                                        Vector2 vector = secondLineV3Arr[i];
                                        Vector2 vector2 = secondLineV3Arr[i + 1];

                                        GameObject currentColliderObject = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Common.ColliderPrefab]);
                                        currentColliderObject.transform.parent = secondLineObject.transform;
                                        currentColliderObject.transform.localPosition = ((vector + vector2) / 2);
                                        currentColliderObject.transform.right = ((vector2 - vector).normalized);
                                        BoxCollider2D currentBoxCollider2D = currentColliderObject.GetComponent<BoxCollider2D>();
                                        currentBoxCollider2D.size = new Vector3((vector2 - vector).magnitude, (float)_playerController.Line.Thickness, (float)_playerController.Line.Thickness);
                                    }
                                }

                                Rigidbody2D rigidBody = secondLineObject.GetComponent<Rigidbody2D>();
                                SetRigidBodyProperties(rigidBody);

                                secondLineObject.transform.rotation = transformRotation;
                            }
                        }

                        // Calculate Erase resource used
                        if (_playerController.Eraser.ResourceCurrent >= 100f)
                        {
                            _playerController.Eraser.ResourceCurrent -= 100f;
                        }
                        else if (_playerController.Eraser.ResourceCurrent > 0.0f)
                        {
                            _playerController.Eraser.ResourceCurrent = 0.0f;
                        }
                    }
                }
                catch { }
            }
        }

        if ((!Input.GetMouseButton(1) || _playerController.Line.ResourceCurrent <= 0) && _isDrawing)
        {
            _isDrawing = false;
        }
    }
    #endregion Private Methods..
}
