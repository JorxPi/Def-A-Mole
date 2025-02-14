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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect initial click
        {
            HandleClick();
        }

        if (Input.GetMouseButton(0) && isMouseDownOnDefense) // Maintain active state while clicking on defense
        {
            if (!MouseOverDefense()) // Check if the mouse moved away from the defense
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast only against the specified layer (Defense)
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, defenseLayer))
        {
            if (hit.transform == transform) // Check if the click is on this specific defense
            {
                rangeIndicator.SetActive(true); // Activate the range indicator
                isMouseDownOnDefense = true; // Flag that the click started on this defense
            }
        }
    }

    private bool MouseOverDefense()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast only against the specified layer (Defense)
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, defenseLayer))
        {
            // Check if the raycast hit this specific defense
            return hit.transform == transform;
        }

        return false;
    }
}
