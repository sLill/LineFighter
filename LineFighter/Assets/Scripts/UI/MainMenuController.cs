using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    #region Member Variables..
    private GameObject _game;
    private GameObject _networkLobbyPanel;
    private GameObject _settingsPanel;
    #endregion Member Variables..

    #region Events
    void Start()
    {
        _game = GameObject.Find(Fields.GameObjects.Game);

        // Load Assets
        AssetLibrary.LoadBaseAssets();

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
        _game.AddComponent<LoLGameController>();
        SceneManager.LoadScene(Fields.Scenes.SceneTemplate);
    }

    private void MultiplayerButton_Clicked()
    {
        _game.AddComponent<SteamGameController>();
        //_networkLobbyPanel = Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.NetworkLobbyManagerObjectPrefab]);
    }

    private void SettingsButton_Clicked()
    {
        _settingsPanel = Instantiate(AssetLibrary.PrefabAssets[Fields.Assets.Prefabs.UI.SettingsMenuPrefab]);
    }
    #endregion Events

    #region Private Methods
    private void InitializeMenu()
    {

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
