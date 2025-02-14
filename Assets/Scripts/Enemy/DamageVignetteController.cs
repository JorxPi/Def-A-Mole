using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DamageVignetteController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image vignetteImage;
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private float maxAlpha = 1f;
    [SerializeField] private float minAlpha = 0f;

    private List<GameObject> enemiesOutside = new List<GameObject>();
    private bool isBlinking = false;
    private float currentAlpha = 0f;

    private void Start()
    {
        // Asegurarse de que la viñeta empiece invisible
        SetVignetteAlpha(0f);
    }

    private void Update()
    {
        // Verificar si hay enemigos fuera
        if (enemiesOutside.Count > 0 && !isBlinking)
        {
            isBlinking = true;
        }
        else if (enemiesOutside.Count == 0 && isBlinking)
        {
            isBlinking = false;
            SetVignetteAlpha(0f);
            
        }

        // Manejar el parpadeo
        if (isBlinking)
        {
            float newAlpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            newAlpha = Mathf.Lerp(minAlpha, maxAlpha, newAlpha);
            SetVignetteAlpha(newAlpha);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemiesOutside.Contains(other.gameObject))
        {
            enemiesOutside.Add(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesOutside.Remove(other.gameObject);
        }
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        if (enemiesOutside.Contains(enemy))
        {
            enemiesOutside.Remove(enemy);
        }
    }

    private void SetVignetteAlpha(float alpha)
    {
        Color color = vignetteImage.color;
        color.a = alpha;
        vignetteImage.color = color;
    }
}