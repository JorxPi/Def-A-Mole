using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Defense))]
public class RangeIndicator : MonoBehaviour
{
    public GameObject rangeIndicatorPrefab; // Assign a range indicator prefab (e.g., a circular sprite or mesh)
    public Transform feetPosition; // Assign the "FeetPosition" child object here
    private GameObject rangeIndicator;
    private Defense defense;

    private void Start()
    {
        defense = GetComponent<Defense>();

        // Instantiate and set up the range indicator
        if (rangeIndicatorPrefab != null && feetPosition != null)
        {
            rangeIndicator = Instantiate(rangeIndicatorPrefab, feetPosition.position, Quaternion.identity, transform);

            // Adjust scale to match range on X and Z axes only
            rangeIndicator.transform.localScale = new Vector3(defense.attackRange * 2, 0.1f, defense.attackRange * 2);
            rangeIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // 0 is the left mouse button
        {
            // Check if the mouse is over this object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                // Show the range indicator if clicking on the object
                if (rangeIndicator != null)
                {
                    rangeIndicator.SetActive(true);
                }
            }
        }
        else
        {
            // Hide the range indicator when the mouse button is released
            if (rangeIndicator != null)
            {
                rangeIndicator.SetActive(false);
            }
        }
    }
}
