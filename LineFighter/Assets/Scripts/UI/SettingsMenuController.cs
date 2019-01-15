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
    private Text _savedText;
    
    #region MonoBehaviour
    void Start()
    {
        InitializeMenuButtonEvents();
        InitializeUI();
    }
    #endregion MonoBehaviour

    #region Events
    private void ApplyButton_Clicked()
    {
        SaveAndEncodeSettings();
    }

    private void CancelButton_Clicked()
    {
        _savedText.text = string.Empty;
        this.gameObject.SetActive(false);
    }
    #endregion Events

    #region Public Methods
    private void SaveAndEncodeSettings()
    {
        try
        {
            string settingsFile = string.Empty;

            // Display
            settingsFile += (Fields.Settings.FrameRateCap + "=" + DisplaySettings.FrameRateCap + "\n");
            settingsFile += (Fields.Settings.VSyncEnabled + "=" + DisplaySettings.VSyncEnabled + "\n");

            // Hud
            settingsFile += (Fields.Settings.FpsCounterActive + "=" + HudSettings.FpsCounterActive + "\n");

            // Keyboard
            settingsFile += (Fields.Settings.DrawModeKey + "=" + KeyboardSettings.DrawModeKey + "\n");

            string path = Path.Combine(Application.persistentDataPath, "Config.cfg");
            File.WriteAllText(path, settingsFile);

            _savedText.text = "Saved";
        }
        catch
        {
            _savedText.text = "Failed";
        }
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
                case Fields.GameObjects.ApplyButton:
                    menuButton.onClick.AddListener(ApplyButton_Clicked);
                    break;
                case Fields.GameObjects.CancelButton:
                    menuButton.onClick.AddListener(CancelButton_Clicked);
                    break;
            }
        }
    }

    private void InitializeUI()
    {
        _savedText = GameObject.Find(Fields.GameObjects.SavedText).GetComponent<Text>();
    }
    #endregion Private Methods
}
