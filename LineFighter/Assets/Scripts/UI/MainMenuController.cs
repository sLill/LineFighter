using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    #region MonoBehaviour
    void Start()
    {
        InitializeMenuButtonEvents();
    }
    #endregion MonoBehaviour

    #region Events

    private void AboutButton_Clicked()
    {

    }

    private void ExitButton_Clicked()
    {

    }

    private void LordsOfLineButton_Clicked()
    {

    }

    private void MultiplayerButton_Clicked()
    {

    }

    private void SettingsButton_Clicked()
    {

    }
    #endregion Events

    #region Private Methods
    private void InitializeMenuButtonEvents()
    {
        Button[] menuButtons = GameObject.FindObjectsOfType<Button>();

        foreach (Button menuButton in menuButtons)
        {
            switch (menuButton.name)
            {
                case Fields.GameObjects.LordsOfLineButton:
                    menuButton.onClick.AddListener(LordsOfLineButton_Clicked);
                    break;
                case Fields.GameObjects.MultiplayerButton:
                    menuButton.onClick.AddListener(MultiplayerButton_Clicked);
                    break;
                case Fields.GameObjects.SettingsButton:
                    menuButton.onClick.AddListener(SettingsButton_Clicked);
                    break;
                case Fields.GameObjects.AboutButton:
                    menuButton.onClick.AddListener(AboutButton_Clicked);
                    break;
                case Fields.GameObjects.ExitButton:
                    menuButton.onClick.AddListener(ExitButton_Clicked);
                    break;
            }
        }
    }
    #endregion Private Methods
}
