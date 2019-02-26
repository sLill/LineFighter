using UnityEngine;
using UnityEngine.Networking;

public class Player : IPlayer
{
    public int ConnectionId { get; set; }

    public bool IsLocalPlayer { get; set; }

    public NetworkInstanceId NetId { get; set; }

    public string PlayerTag { get; set; }
}

