﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IEnemy
{
    #region Member Variables..
    private bool _damageTaken = false;
    private bool _flashRed = false;
    private float _flashingFor = 0f;
    private SpriteRenderer _spriteRenderer;
    #endregion Member Variables..

    #region Properties..
    public float Hp { get; set; }

    public EnemyState State { get; set; }
    #endregion Properties..

    #region Events..
    #region MonoBehaviour..
    public virtual void Awake()
    {
        _spriteRenderer = this.GetComponentInParent<SpriteRenderer>();
        State = EnemyState.Idle;
    }
    public virtual void Start()
    {
        InitializeProperties();
    }

    public virtual void Update()
    {
        if (_damageTaken)
        {
            _damageTaken = false;
            _flashRed = true;
        }

        if (_flashRed &&_flashingFor < 0.1f)
        {
            _flashingFor += Time.deltaTime;
        }
        else if (_flashRed)
        {
            _spriteRenderer.color = Color.white;
            _flashRed = false;
            _flashingFor = 0f;
        }

        // Ded
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    public virtual void InitializeProperties()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        Hp -= damage;
        _damageTaken = true;
        _spriteRenderer.color = Color.red;
    }
    #endregion Public Methods..

    #region Private Methods..
    #endregion Private Methods..
}
