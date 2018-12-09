using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    EventSystem _eventSystem;

    void Start()
    {
        _eventSystem = GameObject.Find(Fields.GameObjects.EventSystem).GetComponent<EventSystem>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Text textObject = eventData.pointerEnter.GetComponentInChildren<Text>();
        textObject.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _eventSystem.SetSelectedGameObject(null);
        Text textObject = eventData.pointerEnter.GetComponentInChildren<Text>();
        textObject.color = Color.white;
    }

}
