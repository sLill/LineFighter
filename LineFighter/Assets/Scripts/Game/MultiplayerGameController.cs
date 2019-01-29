using Assets.Scripts.Network;
using Assets.Scripts.Properties;
using System;
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
    public List<PlayerProfile> PlayerList = new List<PlayerProfile>();
    #endregion Properties..

    #region Events..
    private void Awake()
    {       
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
            PlayerName = "RippStudwell"
        };

        this.PlayerList.Add(playerProfile);
    }
    #endregion Public Methods..

    #region Private Methods..
    private void InitDisplaySettings()
    {
        QualitySettings.vSyncCount = DisplaySettings.VSyncEnabled;
        Application.targetFrameRate = DisplaySettings.FrameRateCap;
    }
    #endregion Private Methods..
}

