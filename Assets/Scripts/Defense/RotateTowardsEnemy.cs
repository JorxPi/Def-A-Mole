using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DBrain))]
public class RotateTowardsEnemy : MonoBehaviour
{
    public Transform mapCenter; // Assign a Transform for the map's center point
    public float rotationSpeed = 5f;

    private DBrain brain;
    private Transform target;

    private GameObject defenseModel;

    private void Start()
    {
        brain = GetComponent<DBrain>();
        defenseModel = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        UpdateTarget();
        LookAtTarget();
    }

    private void UpdateTarget()
    {
        if (brain != null && brain.Target != null)
        {
            target = brain.Target;
        }
        else
        {
            target = FindClosestEnemy() ?? mapCenter;
        }
    }

    private void LookAtTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Lock rotation to horizontal
        defenseModel.transform.rotation = Quaternion.Slerp(defenseModel.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private Transform FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, brain.Defense.attackRange); // Use brain.Defense for range
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = collider.transform;
                }
            }
        }

        return closestEnemy;
    }
}