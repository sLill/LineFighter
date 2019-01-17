public interface IPlayer
{
    bool IsLocalPlayer { get; set; }

    short PlayerControllerId { get; set; }

    int PlayerNumber { get; set; }

    string PlayerTag { get; set; }
}
