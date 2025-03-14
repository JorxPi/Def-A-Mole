using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefenseButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public DefenseData defenseData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (defenseData != null)
        {
            DefenseInfoUI.instance.ShowInfo(defenseData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DefenseInfoUI.instance.HideInfo();
    }
}
