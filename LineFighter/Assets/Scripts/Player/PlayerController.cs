using Assets.Scripts.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Steamworks;

public class PlayerController : Player
{
    #region Member Variables
    private bool _isGrounded = false;
    private bool _queueJump = false;
    private float _time = 0;
    private Animator _animator;
    private List<Direction> _keysDown;
    private Rigidbody2D _rigidBody;
    #endregion Member Variables

    #region Public Properties

    #endregion Public Properties

    #region Events..
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        _animator = this.GetComponentInParent<Animator>();
        _rigidBody = this.GetComponentInParent<Rigidbody2D>();
        _rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Used like a Queue, except elements can be removed at various indexes
        _keysDown = new List<Direction>() { Direction.None };
    }

    public override void Update()
    {
        base.Update();

        // Movement
        if (Input.GetKeyDown(KeyCode.D))
        {
            _keysDown.Add(Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _keysDown.Add(Direction.Left);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _keysDown.Remove(Direction.Right);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _keysDown.Remove(Direction.Left);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _queueJump = true;
        }

        // Increment Draw/Erase Gauges
        if (Line.AutoRefill && _time > .1f)
        {
            if (Line.ResourceCurrent < Line.ResourceMax)
            {
                Line.ResourceCurrent += Line.RefillRate;
            }

            if (Eraser.ResourceCurrent < Eraser.ResourceMax)
            {
                Eraser.ResourceCurrent += Eraser.RefillRate;
            }

            _time = 0f;
        }
        else
        {
            _time += Time.deltaTime;
        }

        // Fire weapon
        if (Input.GetMouseButtonDown(0))
        {
            FireWeapon();
        }
    }

    private void FixedUpdate()
    {
        // Jump
        if (_isGrounded && _queueJump)
        {
            _queueJump = false;
            _isGrounded = false;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Moving = true;
                _rigidBody.velocity = Vector2.zero;
                _rigidBody.AddForce(new Vector2(4.5f, 4.5f), ForceMode2D.Impulse);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Moving = true;
                _rigidBody.velocity = Vector2.zero;
                _rigidBody.AddForce(new Vector2(-4.5f, 4.5f), ForceMode2D.Impulse);
            }
            else
            {
                _rigidBody.velocity = Vector2.zero;
                _rigidBody.AddForce(new Vector2(0.0f, 4.5f), ForceMode2D.Impulse);
            }
        }

        // Movement
        Direction mostRecentDirection = _keysDown[_keysDown.Count - 1];
        if (mostRecentDirection == Direction.Right)
        {
            if (!_isGrounded)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x < 4.5 ? _rigidBody.velocity.x + 1.5f : _rigidBody.velocity.x, _rigidBody.velocity.y);
            }
            else
            {
                _rigidBody.MovePosition(new Vector2(_rigidBody.transform.position.x + 0.06f, _rigidBody.transform.position.y));
            }

            Moving = true;
            Speed = Input.GetAxis("Horizontal");
        }
        else if (mostRecentDirection == Direction.Left)
        {
            if (!_isGrounded)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x > -4.5 ? _rigidBody.velocity.x - 1.5f : _rigidBody.velocity.x, _rigidBody.velocity.y);
            }
            else
            {
                _rigidBody.MovePosition(new Vector2(_rigidBody.transform.position.x - 0.06f, _rigidBody.transform.position.y));
            }

            Moving = true;
            Speed = Input.GetAxis("Horizontal");
        }
        else if (mostRecentDirection == Direction.None)
        {
            Moving = false;
            Speed = 0.00f;
        }     

        // Set animation fields
        _animator.SetFloat(Fields.Animator.Speed, Speed);
        _animator.SetBool(Fields.Animator.Moving, Moving);
        _animator.SetBool(Fields.Animator.Airborne, !_isGrounded);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.contacts[0].point;
        Vector2 playerCenter = _rigidBody.worldCenterOfMass;

        float collisionAngle = Mathf.Atan2(playerCenter.y - contactPoint.y, playerCenter.x - contactPoint.x) * 180 / Mathf.PI;

        // Only ground ourselves if the angle isn't too steep
        if (collisionAngle > 30 && collisionAngle < 150)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Fields.Tags.DrawFuel)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == Fields.Tags.EraseFuel)
        {
            Destroy(collision.gameObject);
        }
        else
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }
    #endregion Events..

    #region Public Methods..

    #endregion Public Methods..

    #region Private Methods

    #endregion Private Methods
}
