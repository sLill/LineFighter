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

    public virtual void FixedUpdate()
    {
        this.transform.Translate(new Vector2(0, Speed));
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case Fields.Tags.Player:
                var playerController = collider.gameObject.GetComponent<IPlayer>();
                playerController.TakeDamage(Damage);
                break;
            case Fields.Tags.Enemy:
                var enemyController = collider.gameObject.GetComponent<IEnemy>();
                enemyController.TakeDamage(Damage);
                break;
            case Fields.Tags.LineObject:
                var lineController = collider.gameObject.GetComponent<ILine>();
                lineController.TakeDamage(this);
                break;
        }

        Destroy(this.gameObject);
    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    #endregion Public Methods..

    #region Private Methods..
    #endregion Private Methods..
}
