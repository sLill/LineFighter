namespace Assets.Scripts.Properties
{
    public class Player : IPlayer
    {
        public bool IsLocalPlayer { get; set; }

        public short PlayerControllerId { get; set; }

        public int PlayerNumber { get; set; }

        public string PlayerTag { get; set; }
    }
}
