using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterBulletController : Projectile
{
    #region Member Variables..
    #endregion Member Variables..

    #region Properties..
    #endregion Properties..

    #region Events..
    #region MonoBehaviour..
    public override void Awake()
    {
        Damage = 10f;
        Speed = 5.0f;

        base.Awake();
    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    #endregion Public Methods..

    #region Private Methods..
    #endregion Private Methods..
}
