using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{ 
    #region Member Variables..'
    private float _gravityConstant = 0.125f;
    private bool _isColliding;
    private EdgeCollider2D _lineCollider;
    private LineRenderer _lineRenderer;
    private Rigidbody2D _rigidBody;
    #endregion Member Variables..

    #region Properties..
    public bool IsComplete { get; set; }
    #endregion Properties..

    #region Events..
    private void Awake()
    {
        _lineCollider = this.GetComponentInParent<EdgeCollider2D>();
        _lineRenderer = this.GetComponentInParent<LineRenderer>();
        _rigidBody = this.GetComponentInParent<Rigidbody2D>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Gravity
        if (IsComplete && !_isColliding)
        {
            _rigidBody.MovePosition(_rigidBody.position - new Vector2(0f, _gravityConstant));
        }
    }
    #endregion Events..
}
