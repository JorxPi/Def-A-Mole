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

    //private List<GameObject> enemiesOutside = new List<GameObject>();
    private int enemiesAttacking = 0;
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
        if (enemiesAttacking > 0 && !isBlinking)
        {
            isBlinking = true;
        }
        else if (enemiesAttacking == 0 && isBlinking)
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

    public void SumEnemyAttacking()
    {
        enemiesAttacking++;
    }

    public void DecreaseEnemiesAttacking()
    {
        if (enemiesAttacking > 0)
        {
            enemiesAttacking--;
        }
    }

    private void SetVignetteAlpha(float alpha)
    {
        Color color = vignetteImage.color;
        color.a = alpha;
        vignetteImage.color = color;
    }
}