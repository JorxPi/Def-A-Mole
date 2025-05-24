using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartScreenUIManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;

    private const string VolumeKey = "fullVolume";
    private const string VolumeParam = "fullVolume";

    private void Start()
    {
        if (PlayerPrefs.HasKey(VolumeKey))
            LoadVolume();
        else
            SetMusicVolume();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void FullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void ChangeQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Call this from your Slider's OnValueChanged(float) event.
    /// </summary>
    public void OnVolumeSliderChanged(float sliderValue)
    {
        AudioManager.Instance.SetMasterVolume(sliderValue);
    }

    private void SetMusicVolume()
    {
        float saved = musicSlider.value;
        // same conversion as above
        float dB = Mathf.Log10(Mathf.Clamp01(saved)) * 20f;
        audioMixer.SetFloat(VolumeParam, dB);
        PlayerPrefs.SetFloat(VolumeKey, saved);
    }

    private void LoadVolume()
    {
        float saved = PlayerPrefs.GetFloat(VolumeKey);
        musicSlider.value = saved;
        SetMusicVolume();
    }
}
