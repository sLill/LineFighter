using Assets.Scripts.Network;
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

        if (_gameController == null)
        {
            _gameController = GameObject.FindObjectOfType<MultiplayerGameController>();
        }
        
        _gameController.AddPlayer(conn);
        Debug.Log("PLAYER CONNECTED");

    }

    // Occurs when any player (including the host) joins a lobby
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        client.RegisterHandler(NetworkMessageTypes.LineObjectMessage, ClientSpawnLine);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        _gameController.PlayerList.Clear();
    }
    #endregion NetworkLobbyManager..

    public void ClientSpawnLine(NetworkMessage msg)
    {
        LineObjectMessage lineObjectMessage = new LineObjectMessage() { MessageType = LineObjectMessage.MsgType.Spawn };
        lineObjectMessage.Deserialize(msg.reader);

        GameObject playerLinesObject = GameObject.FindGameObjectsWithTag(Fields.Tags.PlayerLines).Where(x => x.GetComponent<PlayerController>().netId == lineObjectMessage.SpawnMessage.NetId).FirstOrDefault();
        GameObject lineObject = Instantiate(AssetLibrary.PrefabAssets[Fields.Tags.LineObject]) as GameObject;

        lineObject.transform.parent = playerLinesObject.transform;
    }

    #endregion Events..

    #region Public Methods..
    public void ServerSpawnPlayerLine(GameObject lineObject)
    {
        //NetworkServer.Spawn(lineObject);
        LineObjectMessage lineObjectMessage = new LineObjectMessage() { MessageType = LineObjectMessage.MsgType.Spawn };

    }
    #endregion Public Methods..

    #region Private Methods..

    #endregion Private Methods..
}
