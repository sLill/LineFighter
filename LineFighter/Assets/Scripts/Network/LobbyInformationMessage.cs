using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyInformationMessage : MessageBase
{
    public NetworkHash128 AssetId;
    public uint NetId;
    public byte[] Payload;
    public List<PlayerProfile> PlayerList;
    public Vector3 Position;

    public override void Deserialize(NetworkReader reader)
    {
        NetId = reader.ReadPackedUInt32();
        AssetId = reader.ReadNetworkHash128();
        Position = reader.ReadVector3();
        Payload = reader.ReadBytesAndSize();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.WritePackedUInt32(NetId);
        writer.Write(AssetId);
        writer.Write(Position);
        writer.WriteBytesFull(Payload);
    }
}
