using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    #region Member Variables..
    Camera _mainCamera;
    #endregion Member Variables..

    #region Properties..
    public float Damage { get; set; }

    public float Speed { get; set; }
    #endregion Properties..

    #region Events..
    #region MonoBehaviour..
    public virtual void Awake()
    {
        _mainCamera = Camera.main;
    }
    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        var left = (_mainCamera.transform.position.x - 15);
        var right = (_mainCamera.transform.position.x + 15);
        var top = (_mainCamera.transform.position.y + 15);
        var bottom = (_mainCamera.transform.position.y - 15);

        // Destroy projectiles that have left the screen
        if (this.transform.position.x < left
            || this.transform.position.x > right
            || this.transform.position.y < bottom
            || this.transform.position.y > top)
        {
            Destroy(this.gameObject);
        }
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
                if (this.transform.tag != Fields.Tags.PlayerProjectile)
                {
                    playerController.TakeDamage(Damage);
                }
                break;
            case Fields.Tags.Enemy:
                var enemyController = collider.gameObject.GetComponent<IEnemy>();
                if (this.transform.tag != Fields.Tags.EnemyProjectile)
                {
                    enemyController.TakeDamage(Damage);
                }
                break;
            case Fields.Tags.LineObject:
                var lineController = collider.gameObject.GetComponent<ILine>();
                lineController.TakeDamage(this);
                break;
        }

        if (collider.gameObject.transform.tag != Fields.Tags.PlayerProjectile
            && collider.gameObject.transform.tag != Fields.Tags.EnemyProjectile)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion MonoBehaviour.. 
    #endregion Events..

    #region Public Methods..
    #endregion Public Methods..

    #region Private Methods..
    #endregion Private Methods..
}
