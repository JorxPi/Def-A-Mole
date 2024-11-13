using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Defense))]
public class RangeIndicator : MonoBehaviour
{
    public GameObject rangeIndicatorPrefab;
    public Transform feetPosition; 
    private GameObject rangeIndicator;
    private Defense defense;

    private bool isMouseDownOnDefense = false; 

    private void Start()
    {
        defense = GetComponent<Defense>();


        if (rangeIndicatorPrefab != null && feetPosition != null)
        {
            rangeIndicator = Instantiate(rangeIndicatorPrefab, feetPosition.position, Quaternion.identity, transform);

            rangeIndicator.transform.localScale = new Vector3(defense.attackRange * 2, 0.1f, defense.attackRange * 2);
            rangeIndicator.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (rangeIndicator != null)
        {
            rangeIndicator.SetActive(true); 
            isMouseDownOnDefense = true; 
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && isMouseDownOnDefense)
        {
            if (!MouseOverDefense())
            {
                rangeIndicator.SetActive(false); 
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            rangeIndicator.SetActive(false);
            isMouseDownOnDefense = false;
        }
    }

    private bool MouseOverDefense()
    {
        int defenseLayerMask = LayerMask.GetMask("Defense");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, defenseLayerMask))
        {
            return hit.transform == transform;
        }
        return false;
    }
}
