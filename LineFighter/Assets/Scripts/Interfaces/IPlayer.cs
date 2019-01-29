using UnityEngine.Networking;

public interface IPlayer
{
    int ConnectionId { get; set; }

    bool IsLocalPlayer { get; set; }

    NetworkInstanceId NetId { get; set; }

    string PlayerTag { get; set; }
}
