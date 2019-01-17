using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkLobbyManager
{
    #region Member Variables..
    private GameController _gameController;
    #endregion  Member Variables..

    #region Properties..

    #endregion Properties..

    #region Events..
    private void Awake()
    {
        _gameController = GameObject.FindObjectOfType<GameController>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public override void OnLobbyClientConnect(NetworkConnection conn)
    {
        base.OnLobbyClientConnect(conn);

        // Check if this local client is the host. If so, handle all player lobby information changes and update the other lobby clients
        if (NetworkServer.active)
        {
            // Create unique player profile
            PlayerProfile playerProfile = new PlayerProfile() { NetworkConnection = conn, PlayerName = "RippStudwell" };
            _gameController.PlayerList.Add(playerProfile);
        }
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
