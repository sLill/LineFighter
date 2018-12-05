using Assets.Scripts.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Awake()
    {
        // Initialize game and player settings
        // These will be moved over to the Settings screen once it gets finished
        PlayerController playerController = GameObjects.FindObjectOfType(PlayerController);
        playerController.Eraser.Radius = 0.21f;
        playerController.Eraser.RefillRate = 30;
        playerController.Eraser.ResourceMax = 1000;
        playerController.Eraser.Size = Eraser.EraserSize.Small;
        playerController.Line.RefillRate = 30;
        playerController.Line.ResourceMax = 1000;
        playerController.Line.LineGravity = true;
        playerController.Line.LineThickness = 0.13f;
        HudSettings.FpsCounterActive = true;
        DisplaySettings.FrameRateCap = 300;
        DisplaySettings.UseVSync = 0;
    }

    // Use this for initialization
    void Start()
    {
        InitDisplaySettings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitDisplaySettings()
    {
        QualitySettings.vSyncCount = DisplaySettings.UseVSync;
        Application.targetFrameRate = 300;
    }
}

