﻿using Assets.Scripts.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Steamworks;

public class PlayerController : MonoBehaviour
{
    #region Member Variables
    private bool _isGrounded = false;
    private bool _moving = false;
    private bool _queueJump = false;
    private float _speed;
    private float _time = 0;
    private Animator _animator;
    private DrawErase _drawErase;
    private Direction _directionFacing;
    private List<Direction> _keysDown;
    private GameObject _projectiles;
    private GameObject _playerLines;
    private Rigidbody2D _rigidBody;
    #endregion Member Variables

    #region Public Properties
    public Eraser Eraser { get; set; }

    public Line Line { get; set; }

    public Player Player { get; set; }
    #endregion Public Properties

    #region Events..
    private void Awake()
    {
        _projectiles = GameObject.Find(Fields.GameObjects.Projectiles);
    }

    void Start()
    {
        Eraser = new Eraser();
        Line = new Line();
        Player = new Player();

        _animator = this.GetComponentInParent<Animator>();
        _rigidBody = this.GetComponentInParent<Rigidbody2D>();
        _rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Used like a Queue, except elements can be removed at various indexes
        _keysDown = new List<Direction>() { Direction.None };
        _directionFacing = Direction.Right;

        _playerLines = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Player.PlayerLinesPrefab]);
        _playerLines.name = "Player Lines (NetId: " + Player.NetId + ")";

        InitializeProperties();
        Player.IsLocalPlayer = true;

        DrawErase drawLine = _playerLines.AddComponent<DrawErase>();
    }

    void Update()
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
            // Set position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 projectilePosition = Vector3.zero;
            switch (_directionFacing)
            {
                case Direction.Right:
                    var direction = (mousePosition - this.transform.position).normalized;
                    projectilePosition = this.transform.position + (direction * 5);
                    break;
                case Direction.Left:
                    projectilePosition = this.transform.position- new Vector3(0.5f, 0);
                    break;
            }

            GameObject projectile = (GameObject)Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.Common.Bullet]);
            projectile.transform.position = projectilePosition;
            projectile.transform.LookAt(mousePosition, Vector3.forward);
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
                _rigidBody.velocity = Vector2.zero;
                _rigidBody.AddForce(new Vector2(4.5f, 4.5f), ForceMode2D.Impulse);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _moving = true;
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
            _directionFacing = Direction.Right;

            if (!_isGrounded)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x < 4.5 ? _rigidBody.velocity.x + 1.5f : _rigidBody.velocity.x, _rigidBody.velocity.y);
            }
            else
            {
                _rigidBody.MovePosition(new Vector2(_rigidBody.transform.position.x + 0.06f, _rigidBody.transform.position.y));
            }

            _moving = true;
            _speed = Input.GetAxis("Horizontal");
        }
        else if (mostRecentDirection == Direction.Left)
        {
            _directionFacing = Direction.Left;

            if (!_isGrounded)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x > -4.5 ? _rigidBody.velocity.x - 1.5f : _rigidBody.velocity.x, _rigidBody.velocity.y);
            }
            else
            {
                _rigidBody.MovePosition(new Vector2(_rigidBody.transform.position.x - 0.06f, _rigidBody.transform.position.y));
            }

            _moving = true;
            _speed = Input.GetAxis("Horizontal");
        }
        else if (mostRecentDirection == Direction.None)
        {
            _moving = false;
            _speed = 0.00f;
        }     

        // Set animation fields
        _animator.SetFloat(Fields.Animator.Speed, _speed);
        _animator.SetBool(Fields.Animator.Moving, _moving);
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

    private void InitializeProperties()
    {
        this.Line.AutoRefill = true;
        this.Eraser.Radius = 0.21f;
        this.Eraser.RefillRate = 10;
        this.Eraser.ResourceCurrent = 1000;
        this.Eraser.ResourceMax = 1000;
        this.Eraser.Size = EraserSize.Small;
        this.Line.RefillRate = 10;
        this.Line.ResourceCurrent = 1000;
        this.Line.ResourceMax = 1000;
        this.Line.LineGravity = true;
        this.Line.Thickness = 0.1;
    }
    #endregion Private Methods
}
