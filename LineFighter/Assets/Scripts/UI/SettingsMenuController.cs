using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    #region MonoBehaviour
    void Start()
    {
        InitializeMenuButtonEvents();
    }
    #endregion MonoBehaviour

    #region Events
    private void ApplyButton_Clicked()
    {
        SaveAndEncodeSettings();
    }

    private void CancelButton_Clicked()
    {

    }
    #endregion Events

    #region Public Methods
    private void SaveAndEncodeSettings()
    {
        string settingsFile = string.Empty;

        // Display
        settingsFile += (Fields.Settings.FrameRateCap + "=" + DisplaySettings.FrameRateCap);
        settingsFile += (Fields.Settings.VSyncEnabled + "=" + DisplaySettings.VSyncEnabled);

        // Hud
        settingsFile += (Fields.Settings.FpsCounterActive + "=" + HudSettings.FpsCounterActive);

        // Keyboard
        settingsFile += (Fields.Settings.DrawModeKey + "=" + KeyboardSettings.DrawModeKey);

        //string encryptedString = EncryptionEngine.Encrypt(settingsFile);
        string path = Path.Combine(Application.persistentDataPath, "SettingsConfig.cfg");
        File.WriteAllText(path, settingsFile);
    }
    #endregion

    #region Private Methods
    private void InitializeMenuButtonEvents()
    {
        Button[] menuButtons = GameObject.FindObjectsOfType<Button>();

        foreach (Button menuButton in menuButtons)
        {
            switch (menuButton.name)
            {
                case "ApplyButton":
                    menuButton.onClick.AddListener(ApplyButton_Clicked);
                    break;
                case "CancelButton":
                    menuButton.onClick.AddListener(CancelButton_Clicked);
                    break;
            }
        }
    }
    #endregion Private Methods
}
