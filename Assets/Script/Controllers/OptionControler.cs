using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionControler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject optionPanel;

    [Header("Buttons")]
    [SerializeField] private Button muteButton;
    [SerializeField] private Button unmuteButton;
    [SerializeField] private Button postButton;
    [SerializeField] private Button closeButton;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;

    private float sfxVolume = 0f;
    private float musicVolume = 0f;
    private float masterVolume = 0f;

    const string MASTER_KEY = "MasterVolume";
    const string SFX_KEY = "SFXVolume";
    const string MUSIC_KEY = "MusicVolume";

    private void Awake()
    {
        SetButtonsCallbacks();
        masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1);
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1);

        audioMixer.SetFloat(MASTER_KEY, 100 * Mathf.Log10(masterVolume));
        SliderControl(sfxSlider, sfxVolume, SFX_KEY);
        SliderControl(musicSlider, musicVolume, MUSIC_KEY);
    }
    
    private void SetButtonsCallbacks()
    {
        muteButton.onClick.AddListener(delegate { MuteGame(); });
        unmuteButton.onClick.AddListener(delegate { UnMuteGame(); });
        postButton.onClick.AddListener(delegate { TogglePostProssesing(); });
        closeButton.onClick.AddListener(delegate { SavePreferences(); });

        musicSlider.onValueChanged.AddListener(delegate { SliderControl(musicSlider, MUSIC_KEY); });
        sfxSlider.onValueChanged.AddListener(delegate { SliderControl(sfxSlider, SFX_KEY); });
    }

    private void TogglePostProssesing()
    {
        Debug.Log("Post Processing toggle not available");
    }

    private void MuteGame()
    {
        audioMixer.SetFloat(MASTER_KEY, 100 * Mathf.Log10(0.001f));
        muteButton.gameObject.SetActive(false);
        unmuteButton.gameObject.SetActive(true);
    }

    private void UnMuteGame()
    {
        audioMixer.SetFloat(MASTER_KEY, 100 * Mathf.Log10(1));
        muteButton.gameObject.SetActive(true);
        unmuteButton.gameObject.SetActive(false);
    }

    private void SliderControl(Slider slider, string volume)
    {
        float sliderValue = slider.value;
        audioMixer.SetFloat(volume, 100 * Mathf.Log10(sliderValue));
    }

    private void SliderControl(Slider slider, float sliderValue, string volume)
    {
        slider.value = sliderValue;
        audioMixer.SetFloat(volume, 10 * Mathf.Log10(sliderValue));
    }

    private void SavePreferences()
    {
        optionPanel.SetActive(false);
        PlayerPrefs.SetFloat(MASTER_KEY, masterVolume);
        PlayerPrefs.SetFloat(SFX_KEY, sfxSlider.value);
        PlayerPrefs.SetFloat(MUSIC_KEY, musicSlider.value);
        PlayerPrefs.Save();
    }
}

