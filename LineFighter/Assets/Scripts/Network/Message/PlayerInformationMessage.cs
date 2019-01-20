using Assets.Scripts.Properties;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerInformationMessage : MessageBase
{
    #region Properties..
    public int ConnectionId;

    public NetworkInstanceId NetId;

    public string PlayerTag;
    #endregion Properties..

    #region Constructors..
    public PlayerInformationMessage()
    {

    }

    public PlayerInformationMessage(Player player)
    {
        ConnectionId = player.ConnectionId;
        NetId = player.NetId;
        PlayerTag = player.PlayerTag;
    }
    #endregion Constructors..

    #region Public Methods..
    public override void Deserialize(NetworkReader reader)
    {
        ConnectionId = reader.ReadInt32();
        NetId = reader.ReadNetworkId();
        PlayerTag = reader.ReadString();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(ConnectionId);
        writer.Write(NetId);
        writer.Write(PlayerTag);
    }
    #endregion Public Methods..
}