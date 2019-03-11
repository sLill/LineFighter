using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfFunkController : EnemyAI
{
    #region Member Variables..
    private float _staffBurstCd = 0f;
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

        // Staff burst
        if (_staffBurstCd > 2)
        {
            StaffBurst();
            _staffBurstCd = 0;
        }
        else
        {
            _staffBurstCd += Time.deltaTime;
        }
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
    private void StaffBurst()
    {
        // Ring of projectiles
        for (int i = 1; i < 21; i++)
        {
            float ang = i * 360;
            Vector2 pos;
            pos.x = this.gameObject.transform.position.x + 2f;// * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = this.gameObject.transform.position.y;// * Mathf.Cos(ang * Mathf.Deg2Rad);

            FireProjectile(this.gameObject.transform.position, pos);
        }
    }
    #endregion Private Methods..
}
