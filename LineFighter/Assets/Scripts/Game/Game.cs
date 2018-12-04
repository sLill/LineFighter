using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private void Awake()
    {
        // These will be moved over to the Settings screen once it gets finished
        Eraser.Size = Eraser.EraserSize.Small;
        Eraser.Radius = 0.21f;
		Line.UsesGravity = true;
		Line.Thickness = 0.13f;
        HudSettings.FpsCounterActive = true;
        DisplaySettings.FrameRateCap = 300;
        DisplaySettings.UseVSync = 0;
    }

    // Use this for initialization
    void Start ()
    {
		InitDisplaySettings();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void InitDisplaySettings()
    {
        QualitySettings.vSyncCount = DisplaySettings.UseVSync;
        Application.targetFrameRate = 300;
    }
}
