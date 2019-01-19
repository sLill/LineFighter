using System.Collections.Generic;
using UnityEngine.Networking;

public interface IPlayerProfile
{
    string Address { get; set; }

    int ConnectionId { get; set; }

    int HostId { get; set; }

    bool IsLocalProfile { get; set; }

    short PlayerControllerId { get; set; }

    string PlayerName { get; set; }
}
