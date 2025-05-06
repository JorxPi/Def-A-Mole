using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSwarmMovement : MonoBehaviour
{
    public Transform beehive;
    public float minOrbitSpeed = 120f;
    public float maxOrbitSpeed = 150f;
    public float orbitRadius = 2f;
    public float verticalMovementAmplitude = 0.5f;
    public float verticalMovementSpeed = 2f;

    private float orbitSpeed;
    private float angleOffset;
    private float heightOffset;

    void Start()
    {
        orbitSpeed = Random.Range(minOrbitSpeed, maxOrbitSpeed);
        angleOffset = Random.Range(0f, 360f);
        heightOffset = Random.Range(1.5f, 2f);
    }

    void Update()
    {
        if (beehive == null) return;

        // Reverse orbit direction
        float angle = angleOffset - Time.time * orbitSpeed;
        float x = beehive.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
        float z = beehive.position.z + Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;

        float y = beehive.position.y + heightOffset + Mathf.Sin(Time.time * verticalMovementSpeed) * verticalMovementAmplitude;

        transform.position = new Vector3(x, y, z);

        // Face toward hive, but keep upright (no tilt)
        Vector3 direction = beehive.position - transform.position;
        direction.y = 0f; // Ignore height difference

        if (direction.sqrMagnitude > 0.001f) // Avoid zero length
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}


