using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : NetworkLobbyManager
{
    #region Member Variables..
    private GameController _gameController;

    public List<PlayerProfile> PlayerList { get; set; }
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
        PlayerList = new List<PlayerProfile>();
    }

    void Update()
    {

    }

    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        base.OnLobbyServerConnect(conn);

        // Check if this local client is the host. If so, handle all player lobby information changes and update the other lobby clients
        if (NetworkServer.active)
        // Create unique player profile1)
        {
            bool isLocalProfile = conn.hostId == -1;
            PlayerProfile playerProfile = new PlayerProfile()
            {
                Address = conn.address,
                ConnectionId = conn.connectionId,
                HostId = conn.hostId,
                IsLocalProfile = isLocalProfile,
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

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }

    public override void OnLobbyStartClient(NetworkClient lobbyClient)
    {
        if (!NetworkServer.active)
        {
            lobbyClient.RegisterHandler(MessageType.LobbyInformation, OnLobbyInformationMessageReceived);
        }

        base.OnLobbyStartClient(lobbyClient);
    }
    #endregion Events..

    #region Private Methods..
    private void OnLobbyInformationMessageReceived(NetworkMessage netMessage)
    {
        // You can send any object that inherence from MessageBase
        // The client and server can be on different projects, as long as the MyNetworkMessage or the class you are using have the same implementation on both projects
        // The first thing we do is deserialize the message to our custom type

        LobbyInformation lobbyInformationMessage = netMessage.ReadMessage<LobbyInformation>();

        Debug.Log("LOBBY INFORMATION MESSAGE RECEIVED: " + lobbyInformationMessage.PlayerList[0].ConnectionId);
    }

    public void SendLobbyInformation()
    {
        NetworkController networkLobbyController = GameObject.FindObjectOfType<NetworkController>();

        LobbyInformation lobbyInformation = new LobbyInformation();
        lobbyInformation.PlayerList = networkLobbyController.PlayerList;

        Debug.Log("LOBBY INFORMATION MESSAGE SENT: ");

        NetworkServer.SendToAll(MessageType.LobbyInformation, lobbyInformation);
    }
    #endregion Private Methods..
}

public class LobbyInformation : MessageBase
{
    public List<PlayerProfile> PlayerList = new List<PlayerProfile>();

    public override void Deserialize(NetworkReader reader)
    {
        while(reader.Position < reader.Length)
        {
            PlayerProfile playerProfile = new PlayerProfile();

            playerProfile.IsLocalProfile = reader.ReadBoolean();
            playerProfile.Address = reader.ReadString();
            playerProfile.ConnectionId = reader.ReadInt32();
            playerProfile.HostId = reader.ReadInt32();
            //playerProfile.PlayerControllerId = reader.ReadInt32();
            playerProfile.PlayerName = reader.ReadString();

            PlayerList.Add(playerProfile);
        }
    }

    public override void Serialize(NetworkWriter writer)
    {
        foreach (PlayerProfile playerProfile in PlayerList)
        {
            writer.Write(playerProfile.IsLocalProfile);
            writer.Write(playerProfile.Address);
            writer.Write(playerProfile.ConnectionId);
            writer.Write(playerProfile.HostId);
            //writer.Write(playerProfile.PlayerControllerId);
            writer.Write(playerProfile.PlayerName);
        }
    }
}

public static class MessageType
{
    public const short LobbyInformation = 1000;
}
