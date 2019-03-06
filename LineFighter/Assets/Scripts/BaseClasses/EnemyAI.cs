using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IEnemyAI
{
    #region Member Variables..
    private EnemyState _state;
    #endregion Member Variables..

    #region Properties..
    #endregion Properties..

    #region Events..
    #region MonoBehaviour..
    public virtual void Awake()
    {
        _state = EnemyState.Idle;
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
