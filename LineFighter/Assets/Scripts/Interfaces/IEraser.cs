public interface IEraser<T>
{
    float Radius { get; set; }

    // Per 1/10 second
    int RefillRate { get; set; }

    int ResourceCurrent { get; set; }

    int ResourceMax { get; set; }

    T Size { get; set; }
}
