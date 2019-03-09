
using UnityEngine;

public class Line : ILine
{
    #region Properties
    public bool AutoRefill { get; set; }

    public bool UseGravity { get; set; }

    public double Thickness { get; set; }

    public float RefillRate { get; set; }

    public float ResourceCurrent { get; set; }

    public float ResourceMax { get; set; }
    #endregion Properties

    #region Public Methods..
    public void TakeDamage(IProjectile projectile)
    {

    }
    #endregion Public Methods..
}

