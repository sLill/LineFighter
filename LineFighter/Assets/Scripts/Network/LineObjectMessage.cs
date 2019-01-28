using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network
{
    public class LineObjectMessage : MessageBase
    {
        public MsgType MessageType;

        public LineSpawn SpawnMessage = new LineSpawn();

        public LineUpdate UpdateMessage = new LineUpdate();

        public enum MsgType
        {
            Spawn,
            Update
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.SeekZero();

            if (MessageType == MsgType.Spawn)
            {
                writer.StartMessage(NetworkMessageTypes.LineObjectSpawnMessage);

                writer.Write(SpawnMessage.IpAddress);
                writer.Write(SpawnMessage.NetId);
                writer.Write(SpawnMessage.Thickness);
            }
            else if (MessageType == MsgType.Update)
            {
                writer.StartMessage(NetworkMessageTypes.LineObjectUpdateMessage);

                writer.Write(UpdateMessage.AssetId);
                writer.Write(UpdateMessage.IpAddress);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();
                binaryFormatter.Serialize(memoryStream, UpdateMessage.PointList);

                byte[] byteArr = memoryStream.ToArray();
                writer.Write(byteArr.Length);
                writer.WriteBytesFull(byteArr);
            }

            writer.FinishMessage();
        }

        public override void Deserialize(NetworkReader reader)
        {
            reader.SeekZero();

            if (MessageType == MsgType.Spawn)
            {
                SpawnMessage = new LineSpawn();

                SpawnMessage.IpAddress = reader.ReadString();
                SpawnMessage.NetId = reader.ReadNetworkId();
                SpawnMessage.Thickness = reader.ReadDouble();
            }
            else if (MessageType == MsgType.Update)
            {
                UpdateMessage.AssetId = reader.ReadNetworkHash128();
                UpdateMessage.IpAddress = reader.ReadString();

                int byteCount = reader.ReadInt32();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream(reader.ReadBytes(byteCount));

                UpdateMessage.PointList = (List<SerializableVector2>)binaryFormatter.Deserialize(memoryStream);
            }
        }

        [Serializable]
        public partial class LineSpawn
        {
            public string IpAddress;

            public double Thickness;

            public NetworkInstanceId NetId;
        }

        [Serializable]
        public partial class LineUpdate
        {
            public NetworkHash128 AssetId;

            public string IpAddress;

            public List<SerializableVector2> PointList = new List<SerializableVector2>();
        }
    }
}
