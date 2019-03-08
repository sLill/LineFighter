public interface ILine
{
    bool AutoRefill { get; set; }

    bool UseGravity { get; set; }

    double Thickness { get; set; }

    // Per 1/10 second
    float RefillRate { get; set; }

    float ResourceCurrent { get; set; }

    float ResourceMax { get; set; }
}
