using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    public AudioSource musicSource;        // looped music AudioSource
    public AudioClip backgroundMusicClip;  // assign in Inspector

    [Header("SFX")]
    public AudioSource sfxSource;          // one-shot SFX AudioSource
    public AudioClip placeDefenseClip;
    public AudioClip buttonClickClip;
    public AudioClip topoPunchClip;
    public AudioClip defenseShootClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // start your music
            musicSource.clip = backgroundMusicClip;
            musicSource.loop = true;
            musicSource.volume = 0.1f;
            musicSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>Play a one-shot SFX from a clip.</summary>
    public void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, Mathf.Clamp01(volumeScale));
    }

    /// <summary>Convenience overloads for named clips.</summary>
    public void PlayPlaceDefense(float volumeScale = 1f) => PlaySFX(placeDefenseClip, volumeScale);
    public void PlayButtonClick(float volumeScale = 1f) => PlaySFX(buttonClickClip, volumeScale);
    public void PlayTopoPunch(float volumeScale = 1f) => PlaySFX(topoPunchClip, volumeScale);
    public void PlayDefenseShoot(float volumeScale = 1f) => PlaySFX(defenseShootClip, volumeScale);

}

