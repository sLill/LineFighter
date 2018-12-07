using Assets.Scripts.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Member Variables
    private HudController _hudController;
    private PlayerController _playerController;
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
        // These will be moved over to the Settings screen once it gets finished
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _playerController.Eraser.Radius = 0.21f;
        _playerController.Eraser.RefillRate = 30;
        _playerController.Eraser.ResourceMax = 1000;
        _playerController.Eraser.Size = Eraser.EraserSize.Small;
        _playerController.Line.RefillRate = 30;
        _playerController.Line.ResourceMax = 1000;
        _playerController.Line.LineGravity = true;
        _playerController.Line.Thickness = 0.13f;

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

