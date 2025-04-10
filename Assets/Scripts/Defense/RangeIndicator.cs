using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Defense))]
public class RangeIndicator : MonoBehaviour
{
    public GameObject rangeIndicatorPrefab;
    public Transform baseTransform; // Manually assign Base in Unity Inspector
    public LayerMask baseLayer;

    private GameObject rangeIndicator;
    private Defense defense;
    private bool isMouseDownOnDefense = false;

    private void Start()
    {
        defense = GetComponent<Defense>();

        if (baseTransform == null)
        {
            Debug.LogError($"Base is not assigned in {gameObject.name}. Assign it in the Inspector.");
            return;
        }

        if (rangeIndicatorPrefab != null)
        {
            rangeIndicator = Instantiate(rangeIndicatorPrefab, baseTransform.position, Quaternion.identity, transform);
            rangeIndicator.transform.localScale = new Vector3(defense.attackRange * 2, 0.1f, defense.attackRange * 2);
            rangeIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect initial click
        {
            HandleClick();
        }

        if (Input.GetMouseButton(0) && isMouseDownOnDefense) // Keep active state while clicking on Base
        {
            if (!MouseOverDefense()) // Hide range if mouse moves away
            {
                rangeIndicator.SetActive(false);
                isMouseDownOnDefense = false;
            }
        }

        if (Input.GetMouseButtonUp(0)) // Detect click release
        {
            rangeIndicator.SetActive(false);
            isMouseDownOnDefense = false;
        }
    }

    private void HandleClick()
    {
        if (baseTransform == null) return; // Prevent errors if Base is missing

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, baseLayer))
        {
            Debug.Log($"Clicked on: {hit.transform.name} (Layer: {LayerMask.LayerToName(hit.transform.gameObject.layer)})");

            if (hit.transform.gameObject == baseTransform.gameObject)
            {
                if (rangeIndicator != null)
                {
                    rangeIndicator.SetActive(true);
                }
                isMouseDownOnDefense = true;
            }
        }
    }

    private bool MouseOverDefense()
    {
        if (baseTransform == null) return false; // Prevent errors

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, baseLayer))
        {
            Debug.Log($"Mouse Over: {hit.transform.name}");
            return hit.transform.gameObject == baseTransform.gameObject;
        }

        return false;
    }
}

