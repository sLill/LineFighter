using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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

}
