using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    #region Member Variables..
    #endregion Member Variables..

    #region Properties..
    public float Damage { get; set; }

    public float Speed { get; set; }
    #endregion Properties..

    #region Events..
    #region MonoBehaviour..
    public virtual void Awake()
    {

    }
    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    #endregion Public Methods..

    #region Private Methods..
    #endregion Private Methods..
}
