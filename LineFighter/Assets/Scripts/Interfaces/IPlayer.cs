using UnityEngine.Networking;

public interface IPlayer
{
    int ConnectionId { get; set; }

    bool IsLocalPlayer { get; set; }

    int PlayerNumber { get; set; }

    string PlayerTag { get; set; }
}
