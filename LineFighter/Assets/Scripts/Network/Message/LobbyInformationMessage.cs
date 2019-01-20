using System.Collections.Generic;
using UnityEngine.Networking;

public class LobbyInformationMessage : MessageBase
{
    public List<PlayerProfile> PlayerList = new List<PlayerProfile>();

    public override void Deserialize(NetworkReader reader)
    {
        while (reader.Position < reader.Length)
        {
            PlayerProfile playerProfile = new PlayerProfile();

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
            writer.Write(playerProfile.Address);
            writer.Write(playerProfile.ConnectionId);
            writer.Write(playerProfile.HostId);
            //writer.Write(playerProfile.PlayerControllerId);
            writer.Write(playerProfile.PlayerName);
        }
    }
}
