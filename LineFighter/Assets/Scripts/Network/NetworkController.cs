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

        RegisterHandlers();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        _gameController.PlayerList.Clear();
    }
    #endregion NetworkLobbyManager..

    private void ClientSpawnPlayerLine(NetworkMessage msg)
    {
        LineObjectMessage lineObjectMessage = new LineObjectMessage() { MessageType = LineObjectMessage.MsgType.Spawn };
        lineObjectMessage.Deserialize(msg.reader);

        if (lineObjectMessage.SpawnMessage.IpAddress != client.connection.address)
        {
            GameObject playerLinesObject = GameObject.FindGameObjectsWithTag(Fields.Tags.PlayerLines)
                .Where(x => x.GetComponent<NetworkIdentity>().netId == lineObjectMessage.SpawnMessage.NetId)
                .FirstOrDefault();

            GameObject lineObject = Instantiate(AssetLibrary.PrefabAssets[Fields.Tags.LineObject]) as GameObject;
            lineObject.transform.parent = playerLinesObject.transform;

            Debug.Log("CLIENT: LINEOBJECT SPAWN RECEIVED");
        }
    }

    private void ClientUpdatePlayerLine(NetworkMessage msg)
    {
        LineObjectMessage lineObjectMessage = new LineObjectMessage() { MessageType = LineObjectMessage.MsgType.Update };
        lineObjectMessage.Deserialize(msg.reader);

        if (lineObjectMessage.UpdateMessage.IpAddress != client.connection.address)
        {
            GameObject lineObject = GameObject.FindGameObjectsWithTag(Fields.Tags.LineObject)
                .Where(x => x.GetComponent<NetworkIdentity>().assetId.Equals(lineObjectMessage.UpdateMessage.AssetId))
                .FirstOrDefault();

            LineRenderer lineObjectRenderer = lineObject.GetComponent<LineRenderer>();

            int originalPositionCount = lineObjectRenderer.positionCount;
            lineObjectRenderer.positionCount = lineObjectMessage.UpdateMessage.PointList.Count;

            for (int i = 0; i < lineObjectMessage.UpdateMessage.PointList.Count; i++)
            {
                if (i < originalPositionCount-1)
                {
                    if (lineObjectRenderer.GetPosition(i) == Vector3.zero)
                    {
                        lineObjectRenderer.SetPosition(i, (Vector2)lineObjectMessage.UpdateMessage.PointList[i]);
                    }
                }
            }

            Debug.Log("CLIENT: LINEOBJECT UPDATE RECEIVED");
        }
    }

    private void ServerSpawnPlayerLine(NetworkMessage msg)
    {
        LineObjectMessage lineObjectMessage = new LineObjectMessage() { MessageType = LineObjectMessage.MsgType.Spawn };
        lineObjectMessage.Deserialize(msg.reader);

        NetworkServer.SendToAll(NetworkMessageTypes.LineObjectSpawnMessage, lineObjectMessage);
        Debug.Log("SERVER: LINEOBJECT SPAWN SENT");
    }

    private void ServerUpdatePlayerLine(NetworkMessage msg)
    {
        LineObjectMessage lineObjectMessage = new LineObjectMessage() { MessageType = LineObjectMessage.MsgType.Update };
        lineObjectMessage.Deserialize(msg.reader);

        bool result = NetworkServer.SendToAll(NetworkMessageTypes.LineObjectUpdateMessage, lineObjectMessage);
        Debug.Log("SERVER: LINEOBJECT UPDATE SENT. RESULT = " + result);
    }

    #endregion Events..

    #region Public Methods..

    public void ClientSpawnPlayerLine(GameObject lineObject)
    {
        LineObjectMessage lineObjectMessage = new LineObjectMessage() { MessageType = LineObjectMessage.MsgType.Spawn };

        lineObjectMessage.SpawnMessage.IpAddress = client.connection.address;
        lineObjectMessage.SpawnMessage.NetId = lineObject.GetComponent<NetworkIdentity>().netId;
        lineObjectMessage.SpawnMessage.Thickness = lineObject.GetComponent<LineRenderer>().startWidth;

        client.Send(NetworkMessageTypes.LineObjectSpawnMessage, lineObjectMessage);
    }

    public void ClientUpdatePlayerLine(GameObject lineObject, List<SerializableVector2> pointList)
    {
        if (pointList.Count > 0)
        {
            LineObjectMessage lineObjectMessage = new LineObjectMessage() { MessageType = LineObjectMessage.MsgType.Update };
            lineObjectMessage.UpdateMessage.PointList.AddRange(pointList);
            lineObjectMessage.UpdateMessage.AssetId = lineObject.GetComponent<NetworkIdentity>().assetId;
            lineObjectMessage.UpdateMessage.IpAddress = client.connection.address;

            client.Send(NetworkMessageTypes.LineObjectUpdateMessage, lineObjectMessage);
        }
    }
    #endregion Public Methods..

    #region Private Methods..
    private void RegisterHandlers()
    {
        if (NetworkServer.active)
        {
            NetworkServer.RegisterHandler(NetworkMessageTypes.LineObjectSpawnMessage, ServerSpawnPlayerLine);
            NetworkServer.RegisterHandler(NetworkMessageTypes.LineObjectUpdateMessage, ServerUpdatePlayerLine);
        }

        client.RegisterHandler(NetworkMessageTypes.LineObjectSpawnMessage, ClientSpawnPlayerLine);
        client.RegisterHandler(NetworkMessageTypes.LineObjectUpdateMessage, ClientUpdatePlayerLine);
    }
    #endregion Private Methods..
}
