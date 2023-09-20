using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolutionSelector : MonoBehaviour {
    Dictionary<string, Resolution> resolutionsDict = new Dictionary<string, Resolution>();
    string curResolutionKey;
    [SerializeField]
    TMP_Dropdown dropdownResolution;
    [SerializeField]
    TMP_Dropdown dropdownDisplayMode;
    string previousResolutionKey;
    bool previousFullscreen;
    [SerializeField]
    private Button applyButton;
    [SerializeField]
    private Button revertButton;
    private bool preselectSet = false;

    void Awake() {
        try {
            Resolution[] resolutions = Screen.resolutions;
            foreach (var res in resolutions) {
                string key = res.width + " x " + res.height;
                resolutionsDict.Add(key, res);
                if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height) {
                    if (previousResolutionKey == null) {
                        previousResolutionKey = key;
                    }
                }
            }
            dropdownResolution.AddOptions(resolutionsDict.Keys.ToList());

            if (!preselectSet) {
                previousFullscreen = Screen.fullScreen;
            } 
            // Preselect dropdown
            dropdownResolution.value = dropdownResolution.options.FindIndex(option => option.text == previousResolutionKey);

            dropdownDisplayMode.value = dropdownDisplayMode.options.FindIndex(option => option.text.ToLower().Contains(previousFullscreen ? "fullscreen" : "window"));

            // Start in Fullscreen
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        } catch (System.Exception e) {
            Debug.Log(e);
        }
    }

    void Update() {
        if (dropdownResolution.options[dropdownResolution.value].text != previousResolutionKey || dropdownDisplayMode.options[dropdownDisplayMode.value].text.ToLower().Contains("fullscreen") != previousFullscreen) {
            applyButton.interactable = true;
            revertButton.interactable = true;
        } else {
            applyButton.interactable = false;
            revertButton.interactable = false;
        }
    }

    public void Revert() {
        dropdownResolution.value = dropdownResolution.options.FindIndex(option => option.text == previousResolutionKey);
        dropdownDisplayMode.value = dropdownDisplayMode.options.FindIndex(option => option.text.ToLower().Contains(previousFullscreen ? "fullscreen" : "window"));
    }

    public void ApplyDisplayChanges() {
        bool isFullscreen = false;
        Resolution res = resolutionsDict[dropdownResolution.options[dropdownResolution.value].text];

        if (dropdownDisplayMode.options[dropdownDisplayMode.value].text.ToLower().Contains("fullscreen")) {
            isFullscreen = true;
        }

        previousResolutionKey = dropdownResolution.options[dropdownResolution.value].text;
        previousFullscreen = isFullscreen;

        Screen.SetResolution(res.width, res.height, isFullscreen);
    }

    public void SetDisplaySettings(ResolutionObject resolutionObject) {
        preselectSet = true;
        string keyRes = previousResolutionKey = resolutionObject.width + " x " + resolutionObject.height;
        bool isFullscreen = previousFullscreen = resolutionObject.fullscreen;
        dropdownResolution.value = dropdownResolution.options.FindIndex(option => option.text == keyRes);
        dropdownDisplayMode.value = dropdownDisplayMode.options.FindIndex(option => option.text.ToLower().Contains(isFullscreen ? "fullscreen" : "window"));
    }

    public ResolutionObject GetVideoSettings() {
        Resolution res;
        if (previousResolutionKey != null) {
            res = resolutionsDict[previousResolutionKey];
        } else {
            res = Screen.currentResolution;
        }
        ResolutionObject videoSettings = new ResolutionObject {
            width = res.width,
            height = res.height,
            fullscreen = previousFullscreen
        };
        return videoSettings;
    }
}

[Serializable]
public class ResolutionObject {
    public int width;
    public int height;
    public bool fullscreen;
}
