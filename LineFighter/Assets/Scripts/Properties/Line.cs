
using UnityEngine;

public class Line : ILine
{
    #region Properties
    public bool LineGravity { get; set; }

    public double Thickness { get; set; }

    public int RefillRate { get; set; }

    public int ResourceCurrent { get; set; }

    public int ResourceMax { get; set; }
    #endregion Properties
}

