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
        masterMixer.SetFloat("masterVol", volume);
        AdjustPercentage(masterPercentageText, masterSlider, volume);
    }

    public void SetMusicVolume(float volume) {
        masterMixer.SetFloat("musicVol", volume);
        AdjustPercentage(musicPercentageText, musicSlider, volume);
    }

    public void SetEffectVolume(float volume) {
        masterMixer.SetFloat("soundsVol", volume);
        AdjustPercentage(effectsPercentageText, effectsSlider, volume);
    }

    public void AdjustPercentage(TMP_Text text, Slider slider, float volume) {
        try {
            int range = (int)Mathf.Floor(Mathf.Abs(slider.maxValue - slider.minValue));
            text.text = 100 - Mathf.Floor(Mathf.Abs(volume) / range * 100) + "%";
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