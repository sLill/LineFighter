using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    #region Member Variables..
    private MainMenuNetworkLobbyManagerHud _mainMenuNetworkLobbyManagerHud;
    private NetworkLobbyManager _networkLobbyManager;
    private GameObject _settingsPanel;
    #endregion Member Variables..

    #region Events
    void Start()
    {
        InitializeMenu();
        InitializeMenuButtonEvents();
    }

    private void AboutButton_Clicked()
    {

    }

    private void ExitButton_Clicked()
    {
        Application.Quit();
    }

    private void LordsOfLineButton_Clicked()
    {

    }

    private void MultiplayerButton_Clicked()
    {
        _mainMenuNetworkLobbyManagerHud.ShowGUI = true;
        _networkLobbyManager.showLobbyGUI = true;
    }

    private void SettingsButton_Clicked()
    {
        _settingsPanel.SetActive(true);
    }
    #endregion Events

    #region Private Methods
    private void InitializeMenu()
    {
        _mainMenuNetworkLobbyManagerHud = GameObject.FindObjectOfType<MainMenuNetworkLobbyManagerHud>();
        _networkLobbyManager = GameObject.FindObjectOfType<NetworkLobbyManager>();

        _settingsPanel = GameObject.Find(Fields.GameObjects.SettingsPanel);
        _settingsPanel.SetActive(false);
    }

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
