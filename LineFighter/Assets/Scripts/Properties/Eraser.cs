using UnityEngine;

public class Eraser : IEraser<Eraser.EraserSize>
{
    #region Properties
    public float Radius { get; set; }

    public int RefillRate { get; set; }

    public int ResourceCurrent { get; set; }

    public int ResourceMax { get; set; }

    public EraserSize Size { get; set; }
    #endregion Properties

    #region Enum Types
    public enum EraserSize
    {
        Small,
        Medium,
        Large
    }
    #endregion Enum Types
}
