using Assets.Scripts.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Member Variables..
    private HudController _hudController;
    #endregion Member Variables..

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
    }

    // Use this for initialization
    void Start()
    {
        // Initialize game and player settings
        _hudController = GameObject.FindObjectOfType<HudController>();

        InitDisplaySettings();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion Events..

    #region Private Methods..
    private void InitDisplaySettings()
    {
        QualitySettings.vSyncCount = DisplaySettings.VSyncEnabled;
        Application.targetFrameRate = DisplaySettings.FrameRateCap;
    }
    #endregion Private Methods..
}

