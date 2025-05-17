using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SliderHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI lifeText;

    void Start()
    {
        lifeText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        lifeText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        lifeText.gameObject.SetActive(false);
    }
}
