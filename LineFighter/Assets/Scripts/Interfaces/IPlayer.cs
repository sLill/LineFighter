using UnityEngine.Networking;

public interface IPlayer
{
    float Hp { get; set; }

    bool Moving { get; set; }

    float Speed { get; set; }

    void TakeDamage(float damage);
}
