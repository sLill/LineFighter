public interface ILine
{
    bool AutoRefill { get; set; }

    bool UseGravity { get; set; }

    double Thickness { get; set; }

    float RefillRate { get; set; }

    float ResourceCurrent { get; set; }

    float ResourceMax { get; set; }

    void TakeDamage(IProjectile projectile);
}
