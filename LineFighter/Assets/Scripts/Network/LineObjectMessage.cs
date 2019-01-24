using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Networking;

namespace Assets.Scripts.Network
{
    public class LineObjectMessage : MessageBase
    {
        public MsgType MessageType; 

        public LineSpawn SpawnMessage;

        public LineUpdate UpdateMessage;

        public enum MsgType
        {
            Spawn,
            Update
        }

        public override void Serialize(NetworkWriter writer)
        {
            if (MessageType == MsgType.Spawn)
            {
                writer.Write(SpawnMessage.NetId);
                writer.Write(SpawnMessage.Thickness);
            }
            else if (MessageType == MsgType.Update)
            {

            }
        }

        public override void Deserialize(NetworkReader reader)
        {
            if (MessageType == MsgType.Spawn)
            {
                SpawnMessage = new LineSpawn();

                SpawnMessage.NetId= reader.ReadNetworkId();
                SpawnMessage.Thickness = reader.ReadDouble();
            }
            else if (MessageType == MsgType.Update)
            {

            }
        }

        public partial class LineSpawn
        {
            public NetworkInstanceId NetId;

            public double Thickness;
        }

        public partial class LineUpdate
        {

        }
    }
}
