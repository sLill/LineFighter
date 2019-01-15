public interface IPlayer
{
    bool IsLocalPlayer { get; set; }

    int PlayerNumber { get; set; }

    string PlayerTag { get; set; }
}
