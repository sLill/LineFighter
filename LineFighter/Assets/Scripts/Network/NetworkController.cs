using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkController : NetworkLobbyManager
{
    #region Member Variables..
    private MultiplayerGameController _gameController;
    #endregion  Member Variables..

    #region Properties..
    public NetworkConnection NetworkConnection { get; set; }
    #endregion Properties..

    #region Events..
    
    #region MonoBehaviour..
    private void Awake()
    {
        _gameController = GameObject.FindObjectOfType<MultiplayerGameController>();
    }

    void Start()
    {

    }

    void Update()
    {

    }
    #endregion MonoBehaviour..

    #region NetworkLobbyManager..
    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        base.OnLobbyServerConnect(conn);

        // Host adds the player to the playerlist
        if (NetworkServer.active)
        {
            _gameController.AddPlayer(conn);

            Debug.Log("PLAYER CONNECTED");
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        NetworkConnection = conn;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        _gameController.PlayerList.Clear();
    }
    #endregion NetworkLobbyManager..

    #endregion Events..

    #region Public Methods..

    #endregion Public Methods..

    #region Private Methods..

    #endregion Private Methods..
}
