using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobbyController : NetworkLobbyManager
{
    #region Properties..
    public List<PlayerProfile> PlayerList { get; set; }
    #endregion Properties..

    #region Events..
    void Start()
    {
        PlayerList = new List<PlayerProfile>();
    }

    void Update()
    {

    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        base.OnLobbyClientConnect(conn);

        // Create unique player profile
        PlayerProfile playerProfile = new PlayerProfile() { NetworkConnection = conn, PlayerName = "RippStudwell" };
        PlayerList.Add(playerProfile);
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);

        // Does conn name get updated automatically?
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        base.OnLobbyServerDisconnect(conn);
    }

    public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
    {
        base.OnLobbyServerPlayerRemoved(conn, playerControllerId);
    }

    #endregion Events..
}
