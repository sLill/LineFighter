public interface ILine
{
    bool LineGravity { get; set; }

    float Thickness { get; set; }

    // Per 1/10 second
    int RefillRate { get; set; }

    int ResourceCurrent { get; set; }

    int ResourceMax { get; set; }
}
