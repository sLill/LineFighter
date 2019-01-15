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

    public override void OnLobbyStartServer()
    {
        base.OnLobbyStartServer();
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
    }
    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        base.OnLobbyClientConnect(conn);

        List<short> playerControllerIds = new List<short>();
        foreach (UnityEngine.Networking.PlayerController playerController in conn.playerControllers)
        {
            playerControllerIds.Add(playerController.playerControllerId);
        }

        PlayerList.Add(new PlayerProfile() { NetworkConnection = conn, PlayerControllerIds = playerControllerIds });
    }

    public override void OnLobbyServerDisconnect(NetworkConnection conn)
    {
        base.OnLobbyServerDisconnect(conn);
    }
    #endregion Events..
}
