using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour {
    [SerializeField]
    private AudioMixer masterMixer;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Button applyButton;
    [SerializeField]
    private Button revertButton;
    [SerializeField]
    private TMP_Text masterPercentageText;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private TMP_Text musicPercentageText;
    [SerializeField]
    private Slider effectsSlider;
    [SerializeField]
    private TMP_Text effectsPercentageText;
    private float previousMaster;
    private float previousMusic;
    private float previousEffects;

    void Awake() {
        previousMaster = masterSlider.value;
        previousMusic = musicSlider.value;
        previousEffects = effectsSlider.value;
    }

    void Update() {
        if (masterSlider.value != previousMaster || musicSlider.value != previousMusic || effectsSlider.value != previousEffects) {
            applyButton.interactable = true;
            revertButton.interactable = true;
        } else {
            applyButton.interactable = false;
            revertButton.interactable = false;
        }
    }
    
    public void SetMasterVolume(float volume) {
        masterMixer.SetFloat("masterVol", Mathf.Log10(volume) * 20);
        AdjustPercentage(masterPercentageText, masterSlider, volume);
    }

    public void SetMusicVolume(float volume) {
        masterMixer.SetFloat("musicVol", Mathf.Log10(volume) * 20);
        AdjustPercentage(musicPercentageText, musicSlider, volume);
    }

    public void SetEffectVolume(float volume) {
        masterMixer.SetFloat("soundsVol", Mathf.Log10(volume) * 20);
        AdjustPercentage(effectsPercentageText, effectsSlider, volume);
    }

    public void AdjustPercentage(TMP_Text text, Slider slider, float volume) {
        try {
            float range = slider.maxValue - slider.minValue;
            text.text = Mathf.Floor(volume / range * 100) + "%";
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public AudioSettingsObject GetAudioSettings() {
        AudioSettingsObject audioSettings = new AudioSettingsObject {
            master = masterSlider.value,
            music = musicSlider.value,
            effects = effectsSlider.value
        };
        return audioSettings;
    }

    public void SetSoundSettings(AudioSettingsObject audioSettings) {
        masterSlider.value = previousMaster = audioSettings.master;
        musicSlider.value = previousMusic = audioSettings.music;
        effectsSlider.value = previousEffects = audioSettings.effects;
        SetMasterVolume(audioSettings.master);
        SetMusicVolume(audioSettings.music);
        SetEffectVolume(audioSettings.effects);
    }
    public void Apply() {
        previousMaster = masterSlider.value;
        previousMusic = musicSlider.value;
        previousEffects = effectsSlider.value;
    }

    public void Revert() {
        masterSlider.value = previousMaster;
        musicSlider.value = previousMusic;
        effectsSlider.value = previousEffects;
    }
}

[Serializable]
public class AudioSettingsObject {
    public float master;
    public float music;
    public float effects;
}