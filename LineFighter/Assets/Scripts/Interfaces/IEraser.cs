public interface IEraser<T>
{
    float Radius { get; set; }

    // Per 1/10 second
    int RefillRate { get; set; }

    float ResourceCurrent { get; set; }

    float ResourceMax { get; set; }

    T Size { get; set; }
}
