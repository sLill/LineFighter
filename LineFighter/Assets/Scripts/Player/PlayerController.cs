using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private float _speed;
    private bool _moving = false;
    private bool _queueJump = false;
    private bool _isGrounded = false;
    private List<Direction> _keysDown;

    private enum Direction
    {
        None,
        Up,
        Right,
        Down,
        Left
    }

	void Start ()
    {
        // Used like a Queue, except elements can be removed at various indexes
        _keysDown = new List<Direction>() { Direction.None };

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
	
	void Update ()
    {
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
                _moving = true;
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(new Vector2(4.5f, 4.5f), ForceMode2D.Impulse);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _moving = true;
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(new Vector2(-4.5f, 4.5f), ForceMode2D.Impulse);
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.AddForce(new Vector2(0.0f, 4.5f), ForceMode2D.Impulse);
            }
        }

        // Movement
        Direction mostRecentDirection = _keysDown[_keysDown.Count - 1];
        if (mostRecentDirection == Direction.Right)
        {
            if (!_isGrounded)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x < 4.5 ? _rigidbody.velocity.x + 1.5f : _rigidbody.velocity.x, _rigidbody.velocity.y);
            }
            else
            {
                _rigidbody.MovePosition(new Vector2(_rigidbody.transform.position.x + 0.06f, _rigidbody.transform.position.y));
            }

            _moving = true;
            _speed = Input.GetAxis("Horizontal");
        }
        else if (mostRecentDirection == Direction.Left)
        {
            if (!_isGrounded)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x > -4.5 ? _rigidbody.velocity.x - 1.5f : _rigidbody.velocity.x, _rigidbody.velocity.y);
            }
            else
            {
                _rigidbody.MovePosition(new Vector2(_rigidbody.transform.position.x - 0.06f, _rigidbody.transform.position.y));
            }

            _moving = true;
            _speed = Input.GetAxis("Horizontal");
        }
        else if (mostRecentDirection == Direction.None)
        {
            _moving = false;
            _speed = 0.00f;
        }

        // Fire
        if (Input.GetMouseButton(0))
        {

        }

        // Set animation fields
        _animator.SetFloat(Fields.Animator.Speed, _speed);
        _animator.SetBool(Fields.Animator.Moving, _moving);
        _animator.SetBool(Fields.Animator.Airborne, !_isGrounded);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 contactPoint = collision.contacts[0].point;
        Vector2 playerCenter = _rigidbody.worldCenterOfMass;

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
        _rigidbody.velocity = Vector2.zero;
    }
}
