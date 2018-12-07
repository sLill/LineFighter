using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	// Use this for initialization
	void Start ()
    {
        InitializeMenuButtonEvents();

    }

    #region Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        Text textObject = eventData.pointerEnter.GetComponentInChildren<Text>();
        textObject.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Text textObject = eventData.pointerEnter.GetComponentInChildren<Text>();
        textObject.color = Color.white;
    }

    public void AboutButton_Clicked()
    {

    }

    public void ExitButton_Clicked()
    {

    }

    public void LordsOfLineButton_Clicked()
    {

    }

    public void MultiplayerButton_Clicked()
    {

    }

    public void SettingsButton_Clicked()
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
