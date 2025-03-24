using UnityEngine;
using System.Collections;

public class EnemySpawnEffects : MonoBehaviour
{
    [Header("Scale Animation")]
    public float scaleAnimationDuration = 0.5f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public bool useScaleAnimation = true;

    [Header("Rise Animation")]
    public float riseHeight = 2f;
    public float riseAnimationDuration = 0.5f;
    public AnimationCurve riseCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public bool useRiseAnimation = true;

    [Header("Sound")]
    public AudioClip spawnSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    [Range(0f, 1f)]
    public float spawnSoundVolume = 0.5f;
    [Range(0f, 1f)]
    public float damageSoundVolume = 0.5f;
    [Range(0f, 1f)]
    public float deathSoundVolume = 0.7f;

    [Header("Death Animation")]
    public float deathAnimationDuration = 0.5f;
    public AnimationCurve deathScaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public float deathRotationSpeed = 360f;
    public Color deathColor = Color.red;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private AudioSource audioSource;
    private Damageable damageable;
    private Renderer[] renderers;
    private Color[] originalColors;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        
        // Inicializar el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = spawnSoundVolume;

        // Obtener el componente Damageable
        damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnDamageReceived += PlayDamageSound;
            damageable.OnDeath += PlayDeathEffect;
        }

        // Guardar los renderizadores y colores originales
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        // Configurar la escala inicial
        if (useScaleAnimation)
        {
            transform.localScale = Vector3.zero;
        }

        // Configurar la posición inicial si usamos animación de elevación
        if (useRiseAnimation)
        {
            transform.position = originalPosition - new Vector3(0, riseHeight, 0);
        }

        StartCoroutine(PlaySpawnEffects());
    }

    private void OnDestroy()
    {
        if (damageable != null)
        {
            damageable.OnDamageReceived -= PlayDamageSound;
            damageable.OnDeath -= PlayDeathEffect;
        }
    }

    private IEnumerator PlaySpawnEffects()
    {
        // Reproducir sonido de spawn
        if (spawnSound != null)
        {
            audioSource.clip = spawnSound;
            audioSource.Play();
        }

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < Mathf.Max(scaleAnimationDuration, riseAnimationDuration))
        {
            float scaleProgress = elapsedTime / scaleAnimationDuration;
            float riseProgress = elapsedTime / riseAnimationDuration;

            // Animar escala
            if (useScaleAnimation && scaleProgress <= 1f)
            {
                float scaleValue = scaleCurve.Evaluate(scaleProgress);
                transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, scaleValue);
            }

            // Animar elevación
            if (useRiseAnimation && riseProgress <= 1f)
            {
                float riseValue = riseCurve.Evaluate(riseProgress);
                transform.position = Vector3.Lerp(startPosition, originalPosition, riseValue);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que la escala y posición finales sean exactas
        if (useScaleAnimation) transform.localScale = originalScale;
        if (useRiseAnimation) transform.position = originalPosition;
    }

    private void PlayDamageSound()
    {
        if (damageSound != null && audioSource != null)
        {
            audioSource.volume = damageSoundVolume;
            audioSource.PlayOneShot(damageSound);
        }
    }

    private void PlayDeathEffect()
    {
        // Reproducir sonido de muerte
        if (deathSound != null && audioSource != null)
        {
            audioSource.volume = deathSoundVolume;
            audioSource.PlayOneShot(deathSound);
        }

        // Marcar el enemigo como muriendo en el EnemyAI
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.StartDying();
        }

        // Iniciar animación de muerte
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        // Desactivar colisiones durante la animación de muerte
        // Ya no desactivamos aquí el NavMeshAgent porque lo hacemos en EnemyAI.StartDying()

        // Desactivar colliders durante la animación
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Cambiar color a muerte
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = deathColor;
        }

        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < deathAnimationDuration)
        {
            float progress = elapsedTime / deathAnimationDuration;
            
            // Escalar hacia abajo
            float scaleValue = deathScaleCurve.Evaluate(progress);
            transform.localScale = originalScale * scaleValue;
            
            // Rotar mientras desaparece
            transform.Rotate(Vector3.up, deathRotationSpeed * Time.deltaTime);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que el Enemy.cs maneje la destrucción real del objeto
        // Esto se hace después de que termine la animación
    }
}