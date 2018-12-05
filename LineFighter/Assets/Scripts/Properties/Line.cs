namespace Assets.Scripts.Properties
{
    public class Line : MonoBehaviour, ILine
    {
        #region Properties
        public bool LineGravity { get; set; }

        public float LineThickness { get; set; }

        public int RefillRate { get; set; }

        public int ResourceCurrent { get; set; }

        public int ResourceMax { get; set; }
        #endregion Properties
    }
}
