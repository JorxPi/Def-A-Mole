using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Defense))]
public class RangeIndicator : MonoBehaviour
{
    public GameObject rangeIndicatorPrefab;
    public Transform feetPosition;
    public LayerMask defenseLayer; // Assign the "Defense" layer in the Inspector

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
        // Use a LayerMask to restrict the raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform a raycast that only interacts with the "Defense" layer
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, defenseLayer))
        {
            Debug.Log($"Raycast hit: {hit.transform.name} on layer {LayerMask.LayerToName(hit.transform.gameObject.layer)}");

            // Check if the raycast hit this specific defense
            return hit.transform == transform;
        }

        Debug.Log("Raycast did not hit anything on the Defense layer.");
        return false;
    }
}
