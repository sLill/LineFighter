using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerProfile : IPlayerProfile
{
    #region Properties..
    public string Address { get; set; }

    public int ConnectionId { get; set; }

    public int HostId { get; set; }

    public short PlayerControllerId { get; set; }

    public string PlayerName { get; set; }
    #endregion Properties..

    #region Constructors..
    public PlayerProfile()
    {

    }
    #endregion Constructors..
}
