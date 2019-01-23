using Assets.Scripts.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerGameController : NetworkBehaviour
{
    #region Member Variables..
    private HudController _hudController;
    #endregion Member Variables..

    #region Properties..
    public List<PlayerProfile> PlayerList { get; private set; }
    #endregion Properties..

    #region Events..
    private void Awake()
    {
        // Load Assets
        AssetLibrary.LoadUiAssets();
        AssetLibrary.LoadMaterialAssets();
        AssetLibrary.LoadPrefabAssets();
        
        HudSettings.FpsCounterActive = true;
        DisplaySettings.FrameRateCap = 300;
        DisplaySettings.VSyncEnabled = 0;

        GameObject Game = GameObject.Find(Fields.GameObjects.Game);

        DontDestroyOnLoad(Game);
    }

    void Start()
    {
        // Initialize game and player settings
        _hudController = GameObject.FindObjectOfType<HudController>();

        PlayerList = new List<PlayerProfile>();

        InitDisplaySettings();
    }

    void Update()
    {

    }
    #endregion Events..

    #region Public Methods..
    public void AddPlayer(NetworkConnection conn)
    {
        // Create unique player profile)
        PlayerProfile playerProfile = new PlayerProfile()
        {
            Address = conn.address,
            ConnectionId = conn.connectionId,
            HostId = conn.hostId,
            PlayerName = "RippStudwell",
        };

        this.PlayerList.Add(playerProfile);
        //RpcUpdatePlayerList(this.PlayerList);
    }
    #endregion Public Methods..

    #region Private Methods..
    private void InitDisplaySettings()
    {
        QualitySettings.vSyncCount = DisplaySettings.VSyncEnabled;
        Application.targetFrameRate = DisplaySettings.FrameRateCap;
    }

    //[ClientRpc]
    //private void RpcUpdatePlayerList(List<PlayerProfile> playerList)
    //{
    //    this.PlayerList = playerList;
    //}
    #endregion Private Methods..
}

