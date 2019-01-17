namespace Assets.Scripts.Properties
{
    public static class GameSettings
    {
        #region Properties..
        public static GameMode GameType { get; set; }
        #endregion Properties..

        #region Enums..
        public enum GameMode
        {
            Multiplayer,
            LordsOfLine
        }
        #endregion Enums..
    }
}
