using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    #region Member Variables..
    Rigidbody2D _rigidBody;
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
        _rigidBody = this.GetComponentInParent<Rigidbody2D>();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 LookAtVector = Extensions.LookAtVector(this.transform.position, mousePosition);

        _rigidBody.AddForce(LookAtVector, ForceMode2D.Impulse);
    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    #endregion Public Methods..

    #region Private Methods..
    #endregion Private Methods..
}
