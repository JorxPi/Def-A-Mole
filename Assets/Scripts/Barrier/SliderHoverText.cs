using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SliderHoverText : MonoBehaviour
{
    public TextMeshProUGUI lifeText;

    void Start()
    {
        lifeText.gameObject.SetActive(false);
    }

    public void OnPointerEnter()
    {
        lifeText.gameObject.SetActive(true);
    }

    public void OnPointerExit()
    {
        lifeText.gameObject.SetActive(false);
    }
}
