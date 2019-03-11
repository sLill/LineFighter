using UnityEngine;

public class EraserProperties : IEraser<EraserSize>
{
    #region Properties
    public float Radius { get; set; }

    public int RefillRate { get; set; }

    public float ResourceCurrent { get; set; }

    public float ResourceMax { get; set; }

    public EraserSize Size { get; set; }
    #endregion Properties
}
