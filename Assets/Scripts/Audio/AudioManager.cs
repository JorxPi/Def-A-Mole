using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    public AudioMixer audioMixer;           // assign your Mixer asset
    public string volumeParam = "fullVolume";

    [Header("Music")]
    public AudioSource musicSource;        // looped music AudioSource
    public AudioClip menuMusic;
    public AudioClip gameMusic;  // assign in Inspector

    [Header("SFX")]
    public AudioSource sfxSource;          // one-shot SFX AudioSource
    public AudioClip placeDefenseClip;  
    public AudioClip buttonClickClip;
    public AudioClip topoPunchClip;
    public AudioClip defenseShootClip;
    public AudioClip defenseLevelUpClip;
    public AudioClip noGoldUpgradeClip;
    public AudioClip roundCompleteClip;
    public AudioClip defenseSellClip;
    public AudioClip defenseDestroyedClip;
    public AudioClip slimeShootClip;
    public AudioClip dirtShootClip;
    public AudioClip explosionMoleClip;
    public AudioClip enemySpawnClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

            if (PlayerPrefs.HasKey(volumeParam))
                audioMixer.SetFloat(volumeParam, Mathf.Log10(PlayerPrefs.GetFloat(volumeParam)) * 20);
        }
        else Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip toPlay = null;
        if (scene.name == "StartScreen") toPlay = menuMusic;
        if (scene.name == "SampleScene") toPlay = gameMusic;

        if (toPlay != null && musicSource.clip != toPlay)
        {
            musicSource.Stop();
            musicSource.clip = toPlay;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void SetMasterVolume(float linear)
    {
        // avoid log(0)
        linear = Mathf.Clamp(linear, 0.0001f, 1f);
        float dB = Mathf.Log10(linear) * 20f;
        audioMixer.SetFloat(volumeParam, dB);

        // persist
        PlayerPrefs.SetFloat(volumeParam, linear);
    }


    /// <summary>Play a one-shot SFX from a clip.</summary>
    public void PlaySFX(AudioClip clip, float volumeScale =  1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, Mathf.Clamp01(volumeScale));
    }

    /// <summary>Convenience overloads for named clips.</summary>
    public void PlayPlaceDefense(float volumeScale = 1f) => PlaySFX(placeDefenseClip, volumeScale);
    public void PlayButtonClick(float volumeScale = 1f) => PlaySFX(buttonClickClip, volumeScale);
    public void PlayTopoPunch(float volumeScale = 1f) => PlaySFX(topoPunchClip, volumeScale);
    public void PlayDefenseShoot(float volumeScale = 1f) => PlaySFX(defenseShootClip, volumeScale);
    public void PlayDefenseLevelUp(float volumeScale = 1f) => PlaySFX(defenseLevelUpClip, volumeScale);
    public void PlayNoGoldUpgrade(float volumeScale = 1f) => PlaySFX(noGoldUpgradeClip, volumeScale);
    public void PlayRoundComplete(float volumeScale = 1f) => PlaySFX(roundCompleteClip, volumeScale);
    public void PlayDefenseSell(float volumeScale = 1f) => PlaySFX(defenseSellClip, volumeScale);
    public void PlayDefenseDestroyed(float volumeScale = 1f) => PlaySFX(defenseDestroyedClip, volumeScale);
    public void PlaySlimeShoot(float volumeScale = 1f) => PlaySFX(slimeShootClip, volumeScale);
    public void PlayDirtShoot(float volumeScale = 1f) => PlaySFX(dirtShootClip, volumeScale);
    public void PlayExplosionMole(float volumeScale = 1f) => PlaySFX(explosionMoleClip, volumeScale);
    public void PlayEnemySpawn(float volumeScale = 1f) => PlaySFX(enemySpawnClip, volumeScale);






}

