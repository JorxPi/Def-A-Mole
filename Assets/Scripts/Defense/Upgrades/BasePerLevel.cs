using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePerLevel : MonoBehaviour
{
    [Tooltip("Prefabs bases")]
    public GameObject[] basePrefabs;

    private GameObject currentInstance;

    [Header("Predefined Base")]
    [SerializeField] private Transform initialBaseObject; // arrástrala si tienes una

    private void Start()
    {
        currentInstance = initialBaseObject != null ? initialBaseObject.gameObject : null;
    }

    public void SetBaseLevel(int level)
    {
        if (level < 0 || level >= basePrefabs.Length)
        {
            Debug.LogWarning("Nivel de base fuera de rango.");
            return;
        }

        if (currentInstance != null)
            Destroy(currentInstance);

        // Instanciamos nueva base como hijo
        currentInstance = Instantiate(basePrefabs[level], transform);
        currentInstance.name = "Base";

    }
}
