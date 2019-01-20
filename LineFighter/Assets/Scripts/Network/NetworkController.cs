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
    private GameController _gameController;
    #endregion  Member Variables..

    #region Properties..
    public NetworkConnection NetworkConnection { get; set; }

    public List<PlayerProfile> PlayerList { get; set; }
    #endregion Properties..

    #region Events..
    
    #region MonoBehaviour..
    private void Awake()
    {
        _gameController = GameObject.FindObjectOfType<GameController>();
    }

    void Start()
    {
        PlayerList = new List<PlayerProfile>();
    }

    void Update()
    {

    }
    #endregion MonoBehaviour..

    #region NetworkLobbyManager..
    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        base.OnLobbyServerConnect(conn);

        // Check if this local client is the host. If so, handle all player lobby information changes and update the other lobby clients
        if (NetworkServer.active)
        {
            // Create unique player profile)
            bool isLocalProfile = conn.hostId == -1;
            PlayerProfile playerProfile = new PlayerProfile()
            {
                Address = conn.address,
                ConnectionId = conn.connectionId,
                HostId = conn.hostId,
                PlayerName = "RippStudwell",
            };

            this.PlayerList.Add(playerProfile);
            Debug.Log("PLAYER CONNECTED");

            if (conn.hostId != -1)
            {
                SendLobbyInformation();
            }
        }
    }

    public override void OnLobbyStartClient(NetworkClient lobbyClient)
    {
        if (!NetworkServer.active)
        {
            lobbyClient.RegisterHandler(MessageType.LobbyInformation, OnLobbyInformationMessageReceived);
            lobbyClient.RegisterHandler(MessageType.PlayerInformation, OnClientPlayerInformationReceived);
        }
        else
        {
            NetworkServer.RegisterHandler(MessageType.PlayerInformation, OnServerPlayerInformationReceived);
        }

        base.OnLobbyStartClient(lobbyClient);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        NetworkConnection = conn;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        PlayerList.Clear();
    }
    #endregion NetworkLobbyManager..

    private void OnClientPlayerInformationReceived(NetworkMessage networkMessage)
    {
        PlayerInformationMessage playerInformationMessage = networkMessage.ReadMessage<PlayerInformationMessage>();

        //var g = Extensions.FindObjectsOfTypeAll<PlayerController>();
        //PlayerController playerController = Extensions.FindObjectsOfTypeAll<PlayerController>().Where(x => x.Player.NetId == playerInformationMessage.NetId).FirstOrDefault();
        //playerController.UpdatePlayerInformation(playerInformationMessage);

        var f = SceneManager.GetActiveScene();
        var t = SceneManager.GetAllScenes();
        var g = GameObject. FindGameObjectsWithTag(Fields.Tags.Player);
        //WHY CANT I FIND PLAYER TAGGED OBJECTS AHHHHHHH
        Debug.Log("PLAYER INFORMATION MESSAGE RECEIVED: " + playerInformationMessage.PlayerTag);
    }

    private void OnLobbyInformationMessageReceived(NetworkMessage networkMessage)
    {
        LobbyInformationMessage lobbyInformationMessage = networkMessage.ReadMessage<LobbyInformationMessage>();
        PlayerList = lobbyInformationMessage.PlayerList;

        Debug.Log("LOBBY INFORMATION MESSAGE RECEIVED: " + lobbyInformationMessage.PlayerList[0].ConnectionId);
    }

    private void OnServerPlayerInformationReceived(NetworkMessage networkMessage)
    {
        PlayerInformationMessage playerInformationMessage = networkMessage.ReadMessage<PlayerInformationMessage>();
        ServerSendPlayerInformation(playerInformationMessage);
    }

    #endregion Events..

    #region Public Methods..
    public void ClientSendPlayerInformation(PlayerInformationMessage playerInformationMessage)
    {
        client.Send(MessageType.PlayerInformation, playerInformationMessage);
    }

    public void ServerSendPlayerInformation(PlayerInformationMessage playerInformationMessage)
    {
        NetworkServer.SendToAll(MessageType.PlayerInformation, playerInformationMessage);
    }
    #endregion Public Methods..

    #region Private Methods..
    private void SendLobbyInformation()
    {
        LobbyInformationMessage lobbyInformation = new LobbyInformationMessage();
        lobbyInformation.PlayerList = PlayerList;

        Debug.Log("LOBBY INFORMATION MESSAGE SENT: ");

        NetworkServer.SendToAll(MessageType.LobbyInformation, lobbyInformation);
    }
    #endregion Private Methods..
}
