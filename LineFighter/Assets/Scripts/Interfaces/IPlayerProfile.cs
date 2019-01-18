using System.Collections.Generic;
using UnityEngine.Networking;

public interface IPlayerProfile
{
    bool IsLocalProfile { get; set; }

    NetworkConnection NetworkConnection { get; set; }

    List<short> PlayerControllerIds { get; set; }

    string PlayerName { get; set; }
}
