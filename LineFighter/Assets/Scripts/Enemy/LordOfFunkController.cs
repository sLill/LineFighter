using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfFunkController : EnemyAI
{
    #region Member Variables..
    #endregion Member Variables..

    #region Properties..
    #endregion Properties..

    #region Events..
    #region MonoBehaviour..
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    public override void InitializeProperties()
    {
        base.InitializeProperties();

        this.Hp = 100;
    }
    #endregion Public Methods..

    #region Private Methods..
    #endregion Private Methods..
}
