using System;
using System.Collections;
using AG.Files;
using AG.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour {
    [SerializeField]
    private GameObject tabsParent;
    [SerializeField]
    private GameObject tabContentArea;
    [SerializeField]
    private ScreenResolutionSelector screenResSel;
    [SerializeField]
    private SoundSettings soundSettings;
    private SettingsOptions settingsOptions;
    private FileHandler fh;
    private bool isInit = false;

    void Awake() {
        InitSettings();
    }

    public void InitSettings() {
        if (isInit)
            return;
        fh = ScriptableObject.CreateInstance<FileHandler>();
        string loadedFileData = fh.Load(FileHandler.FileType.Settings);
        if (loadedFileData != null)
        {
            settingsOptions = JsonUtility.FromJson<SettingsOptions>(loadedFileData);
            soundSettings.SetSoundSettings(settingsOptions.audio);
            screenResSel.SetDisplaySettings(settingsOptions.video);
        }
        else
        {
            Debug.Log("No settings file found, using default.");
            settingsOptions = new SettingsOptions()
            {
                audio = soundSettings.GetAudioSettings(),
                video = screenResSel.GetVideoSettings()
            };
        }
        ConvertToJson();
        isInit = true;
    }

    // Load Settings
    void Start() {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            gameObject.SetActive(false);
    }

    public void ChangeTab(Toggle toggle) {
        if (toggle.isOn)
            CheckTabState();
        ToggleContent(toggle);
    }

    private void ToggleContent(Toggle toggle) {
        try {
            GameObject content = tabContentArea.transform.Find(toggle.transform.name + " Content").gameObject;
            if (toggle.isOn) {
                content.SetActive(true);
            } else {
                content.SetActive(false);
            }
        } catch (System.Exception e) {
            Debug.Log(e);
        }
    }

    private void CheckTabState() {
        Toggle[] toggles = tabsParent.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles) {
            GameObject selectedIndicator = toggle.gameObject.transform.GetChild(2).gameObject;
            if (toggle.isOn) {
                selectedIndicator.SetActive(true);
            } else {
                selectedIndicator.SetActive(false);
            }
        }
    }

    // Can't be set in FileHandler, as it won't work there
    private string ConvertToJson() {
        string json = JsonUtility.ToJson(settingsOptions, true);
        return json;
    }

    private IEnumerator SaveCoroutine() {
        yield return new WaitForSecondsRealtime(0.5f);
        settingsOptions = new SettingsOptions() {
            audio = soundSettings.GetAudioSettings(),
            video = screenResSel.GetVideoSettings()
        };
        fh.Save(FileHandler.FileType.Settings, ConvertToJson());
    }

    public void SaveSettingsToFile() {
        StartCoroutine(SaveCoroutine());
    }

    public void CloseSettings() {
        screenResSel.Revert();
        soundSettings.Revert();
        gameObject.SetActive(false);
    }

    public void OpenSettings() {
        gameObject.SetActive(true);
    }
}

[Serializable]
public class SettingsOptions {
    public AudioSettingsObject audio;
    public ResolutionObject video;
}