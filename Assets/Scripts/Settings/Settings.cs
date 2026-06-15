using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    private const string ResolutionIndexKey = "ResolutionIndex";
    private const string FullscreenKey = "Fullscreen";
    private const string HiddenNameKey = "HiddenName";
    private const string HiddenPropertiesKey = "HiddenProperties";
    private const string HiddenNameChampKey = "HiddenNameChamp";
    private const string HiddenDamageKey = "HiddenDamage";
    private const string HiddenChatKey = "HiddenChat";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";
    private const string VoiceVolumeKey = "VoiceVolume";

    public TMP_Dropdown _dropdownResolution;
    public Toggle _toggleFullscreen;
    public Toggle _toggleHiddenName;
    public Toggle _toggleHiddenProperties;
    public Toggle _toggleHiddenNameChamp;
    public Toggle _toggleHiddenDamage;
    public Toggle _toggleHiddenChat;
    public Slider _sliderMusic;
    public Slider _sliderSound;
    public Slider _sliderVoice;
    public Button _buttonSave;

    public AudioSource _backgroundMusic;

    private Resolution[] _resolutions;

    private void Start()
    {
        List<Resolution> validResolutions = new List<Resolution>();
        Resolution[] allResolutions = Screen.resolutions;
        HashSet<string> uniqueResolutions = new HashSet<string>();

        foreach (Resolution res in allResolutions)
        {
            if (Mathf.Approximately(res.width / (float)res.height, 16f / 9f))
            {
                string resolutionString = $"{res.width} x {res.height}";

                if (!uniqueResolutions.Contains(resolutionString))
                {
                    uniqueResolutions.Add(resolutionString);
                    validResolutions.Add(res);
                }
            }
        }

        _resolutions = validResolutions.ToArray();

        _dropdownResolution.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        foreach (Resolution res in validResolutions)
        {
            resolutionOptions.Add($"{res.width} x {res.height}");
        }
        _dropdownResolution.AddOptions(resolutionOptions);

        LoadSettings();

        _sliderMusic.onValueChanged.AddListener((value) => ChangeSliderMusic());
        ChangeSliderMusic();

        _buttonSave.onClick.AddListener(SaveSettings);

        SetKey();
    }

    void SetKey()
    {
        if (PlayerPrefs.GetInt("_left") == 0)
        {
            PlayerPrefs.SetInt("_left", (int)KeyCode.LeftArrow);
        }
        if (PlayerPrefs.GetInt("_right") == 0)
        {
            PlayerPrefs.SetInt("_right", (int)KeyCode.RightArrow);
        }
        if (PlayerPrefs.GetInt("_jump") == 0)
        {
            PlayerPrefs.SetInt("_jump", (int)KeyCode.UpArrow);
        }
        if (PlayerPrefs.GetInt("_fall") == 0)
        {
            PlayerPrefs.SetInt("_fall", (int)KeyCode.DownArrow);
        }
        if (PlayerPrefs.GetInt("_skill01") == 0)
        {
            PlayerPrefs.SetInt("_skill01", (int)KeyCode.Q);
        }
        if (PlayerPrefs.GetInt("_skill02") == 0)
        {
            PlayerPrefs.SetInt("_skill02", (int)KeyCode.W);
        }
        if (PlayerPrefs.GetInt("_skill03") == 0)
        {
            PlayerPrefs.SetInt("_skill03", (int)KeyCode.E);
        }
        if (PlayerPrefs.GetInt("_skill04") == 0)
        {
            PlayerPrefs.SetInt("_skill04", (int)KeyCode.R);
        }
        if (PlayerPrefs.GetInt("_spell") == 0)
        {
            PlayerPrefs.SetInt("_spell", (int)KeyCode.F);
        }
        if (PlayerPrefs.GetInt("_intrinsic") == 0)
        {
            PlayerPrefs.SetInt("_intrinsic", (int)KeyCode.LeftControl);
        }
        if (PlayerPrefs.GetInt("_attack") == 0)
        {
            PlayerPrefs.SetInt("_attack", (int)KeyCode.Space);
        }
    }

    void LoadSettings()
    {
        int resolutionIndex = PlayerPrefs.GetInt(ResolutionIndexKey);
        _dropdownResolution.value = resolutionIndex;
        _toggleFullscreen.isOn = PlayerPrefs.GetInt(FullscreenKey, 1) == 1;
        _toggleHiddenName.isOn = PlayerPrefs.GetInt(HiddenNameKey) == 1;
        _toggleHiddenProperties.isOn = PlayerPrefs.GetInt(HiddenPropertiesKey) == 1;
        _toggleHiddenNameChamp.isOn = PlayerPrefs.GetInt(HiddenNameChampKey) == 1;
        _toggleHiddenDamage.isOn = PlayerPrefs.GetInt(HiddenDamageKey) == 1;
        _toggleHiddenChat.isOn = PlayerPrefs.GetInt(HiddenChatKey) == 1;
        _sliderMusic.value = PlayerPrefs.GetFloat(MusicVolumeKey, 0.3f);
        _sliderSound.value = PlayerPrefs.GetFloat(SoundVolumeKey, 0.5f);
        _sliderVoice.value = PlayerPrefs.GetFloat(VoiceVolumeKey, 0.5f);
        _backgroundMusic.volume = PlayerPrefs.GetFloat(MusicVolumeKey);
        SetResolution(resolutionIndex);
    }

    void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < _resolutions.Length)
        {
            Resolution selectedResolution = _resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, _toggleFullscreen.isOn);
        }
    }

    void ChangeSliderMusic()
    {
        _backgroundMusic.volume = _sliderMusic.value;
    }

    void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", _dropdownResolution.value);
        PlayerPrefs.SetInt("Fullscreen", _toggleFullscreen.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenName", _toggleHiddenName.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenProperties", _toggleHiddenProperties.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenNameChamp", _toggleHiddenNameChamp.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenDamage", _toggleHiddenDamage.isOn ? 1 : 0);
        PlayerPrefs.SetInt("HiddenChat", _toggleHiddenChat.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", _sliderMusic.value);
        PlayerPrefs.SetFloat("SoundVolume", _sliderSound.value);
        PlayerPrefs.SetFloat("VoiceVolume", _sliderVoice.value);
        
        PlayerPrefs.Save();
        LoadSettings();
    }
}