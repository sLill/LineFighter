using Assets.Scripts.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Member Variables
    private HudController _hudController;
    #endregion Member Variables

    private void Awake()
    { 
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

    private void InitDisplaySettings()
    {
        QualitySettings.vSyncCount = DisplaySettings.VSyncEnabled;
        Application.targetFrameRate = DisplaySettings.FrameRateCap;
    }
}

