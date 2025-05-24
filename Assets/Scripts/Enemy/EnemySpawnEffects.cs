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
    [Range(0f, 1f)]
    public float spawnSoundVolume = 0.5f;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private AudioSource audioSource;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        
        // Inicializar el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = spawnSoundVolume;

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

    private IEnumerator PlaySpawnEffects()
    {
        // Reproducir sonido de spawn
        AudioManager.Instance.PlayEnemySpawn(0.5f);

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
}