using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour, IPlayer
{
    #region Member Variables..
    private bool _damageTaken = false;
    private bool _flashRed = false;
    private float _flashingFor = 0f;
    private GameObject _playerLines;
    private GameObject _projectiles;
    private SpriteRenderer _spriteRenderer;
    #endregion Member Variables..

    #region Properties
    public EraserProperties Eraser { get; set; }

    public float Hp { get; set; }

    public LineProperties Line { get; set; }

    public bool Moving { get; set; }

    public float Speed { get; set; }
    #endregion Properties

    #region Events..
    #region MonoBehaviour..
    public virtual void Awake()
    {
        _spriteRenderer = this.GetComponentInParent<SpriteRenderer>();
        _projectiles = GameObject.Find(Fields.GameObjects.Projectiles);
    }
    public virtual void Start()
    {
        Eraser = new EraserProperties();
        Line = new LineProperties();

        InitializeProperties();

        _playerLines = (GameObject)Instantiate(AssetLibrary.PlayerPrefabAssets[Fields.Assets.Prefabs.Player.PlayerLinesPrefab]);
        _playerLines.name = "Player Lines";

        _playerLines.AddComponent<DrawErase>();
    }

    public virtual void Update()
    {
        if (_damageTaken)
        {
            _damageTaken = false;
            _flashRed = true;
        }

        if (_flashRed && _flashingFor < 0.1f)
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
    public void FireWeapon()
    {
        // Set position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 projectilePosition = mousePosition.x > this.transform.position.x ? this.transform.position + new Vector3(0.75f, 0, 0) : this.transform.position - new Vector3(0.75f, 0, 0);

        GameObject projectile = (GameObject)Instantiate(AssetLibrary.PlayerPrefabAssets[Fields.Assets.Prefabs.Common.Bullet]);
        projectile.transform.position = projectilePosition;
        projectile.transform.LookAt(mousePosition, Vector3.forward);
        projectile.transform.parent = _projectiles.transform;
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
        _damageTaken = true;
        _spriteRenderer.color = Color.red;
    }
    #endregion Public Methods..

    #region Private Methods..
    private void InitializeProperties()
    {
        this.Hp = 100;
        this.Line.AutoRefill = true;
        this.Eraser.Radius = 0.21f;
        this.Eraser.RefillRate = 10;
        this.Eraser.ResourceCurrent = 1000;
        this.Eraser.ResourceMax = 1000;
        this.Eraser.Size = EraserSize.Small;
        this.Line.RefillRate = 10;
        this.Line.ResourceCurrent = 1000;
        this.Line.ResourceMax = 1000;
        this.Line.UseGravity = false;
        this.Line.Thickness = 0.1;
    }
    #endregion Private Methods..
}

