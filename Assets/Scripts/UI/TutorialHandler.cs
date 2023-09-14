using System.IO;
using UnityEngine;
using AG.Files;
using System;

public class TutorialHandler : MonoBehaviour {
    [SerializeField]
    TextAsset defaultTutorial;
    private FileHandler fh;
    private TutorialEntry[] tutorialEntries;

    void Start() {
        fh = ScriptableObject.CreateInstance<FileHandler>();
        if (defaultTutorial) {
            tutorialEntries = JsonHelper.FromJson<TutorialEntry>(defaultTutorial.text);
            Debug.Log(tutorialEntries);
        } else {
            Debug.LogError("No default tutorial found");
        }
    }

    public void ResetTutorial() {
        TutorialEntry te = new TutorialEntry {
            name = "Tutorial",
            completed = false
        };
        tutorialEntries[0] = te;
        tutorialEntries[1] = te;
        string json = ConvertToJson();
        Debug.Log(json);
        fh.Save(FileHandler.FileType.Tutorial, json);
    }

    // Can't be set in FileHandler, as it won't work there
    private string ConvertToJson() {
        string json = JsonHelper.ToJson(tutorialEntries, true);
        return json;
    }
}

[Serializable]
public class TutorialEntry {
    public String name;
    public bool completed;
}