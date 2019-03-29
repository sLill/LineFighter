﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour, ILine
{
    #region Member Variables..
    private Camera _mainCamera;
    private ParticleSystem _currentLineFX;
    private ParticleSystem.EmitParams _lineParticleParams = new ParticleSystem.EmitParams();
    #endregion Member Variables..

    #region Properties..
    #endregion Properties..

    #region Events..
    #region MonoBehaviour..
    public virtual void Awake()
    {
        _currentLineFX = this.GetComponent<ParticleSystem>();
        _mainCamera = Camera.main;
    }
    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        var left = (_mainCamera.transform.position.x - 25);
        var right = (_mainCamera.transform.position.x + 25);
        var top = (_mainCamera.transform.position.y + 25);
        var bottom = (_mainCamera.transform.position.y - 25);

        // Destroy projectiles that have left the screen
        if (this.transform.position.x < left
            || this.transform.position.x > right
            || this.transform.position.y < bottom
            || this.transform.position.y > top)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    public void TakeDamage(IProjectile projectile)
    {
        var renderer = this.gameObject.GetComponent<LineRenderer>();
        _currentLineFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Vector3 position;
        for (int i = 0; i < renderer.positionCount; i++)
        {
            position = renderer.transform.TransformPoint(renderer.GetPosition(i));

            _lineParticleParams.position = position;
            _currentLineFX.Emit(_lineParticleParams, 10);
        }
    }
    #endregion Public Methods..

    #region Private Methods..
    #endregion Private Methods..
}
