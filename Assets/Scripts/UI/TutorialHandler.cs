using System.IO;
using UnityEngine;
using AG.Files;
using System;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using AG.UI;

public class TutorialHandler : MonoBehaviour {
    [SerializeField]
    ToggleMenu toggleMenu;
    [SerializeField]
    TextAsset defaultTutorial;
    [SerializeField]
    GameObject tutorialUI;
    [SerializeField]
    TMP_Text tutorialText;
    public static TutorialHandler instance;
    private FileHandler fh;
    private TutorialEntry[] tutorialEntries;
    private Queue<TutorialEntry> tutorialQueue;
    private bool processingQueue = false;
    private TutorialEntry currentTutorialEntry;
    public static bool tutorialActive = false;

    void Start() {
        tutorialQueue = new Queue<TutorialEntry>();
        fh = ScriptableObject.CreateInstance<FileHandler>();
        instance = this;
        string loadedFileData = fh.Load(FileHandler.FileType.Tutorial);
        if (loadedFileData != null) {
            tutorialEntries = JsonHelper.FromJson<TutorialEntry>(loadedFileData);
        } else {
            Debug.Log("No tutorial file found, using default.");
            LoadDefaultTutorial();
        }
        InitTutorials();
    }

    private void InitTutorials() {
        AddTutorialToShow("Tutorial");
    }

    private void LoadDefaultTutorial() {
        tutorialEntries = JsonHelper.FromJson<TutorialEntry>(defaultTutorial.text);
        string json = ConvertToJson();
        fh.Save(FileHandler.FileType.Tutorial, json);
    }

    public void ResetTutorial() {
        tutorialQueue.Clear();
        LoadDefaultTutorial();
        InitTutorials();
    }

    // Can't be set in FileHandler, as it won't work there
    private string ConvertToJson() {
        string json = JsonHelper.ToJson(tutorialEntries, true);
        return json;
    }

    public static void AddTutorialToShow(string key, string requirementKey = null) {
        if (requirementKey != null) {
            TutorialEntry requirement = instance.GetTutorialEntry(requirementKey);
            if (requirement != null) {
                if (!requirement.completed) {
                    return;
                }
            }
        }
        TutorialEntry te = instance.GetTutorialEntry(key);
        if (te != null) {
            if (!te.completed) {
                instance.tutorialQueue.Enqueue(te);
                instance.ShowTutorials();
            }
        } else {
            Debug.LogError("Tutorial entry not found");
        }
    }

    private void ShowTutorials() {
        if (!processingQueue) {
            processingQueue = true;
            if (tutorialQueue.Count > 0) {
                HandleNextEntry();
            }
        }
    }

    private void HandleNextEntry() {
        currentTutorialEntry = tutorialQueue.Dequeue();
        TutorialEntry lastEntry = GetTutorialEntry("LastTutorial");
        if (lastEntry != null) {
            lastEntry.description = currentTutorialEntry.name;
        }
        DisplayTutorial(currentTutorialEntry);
    }

    private void DisplayTutorial(TutorialEntry te) {
        toggleMenu.ToggleWithoutInput(tutorialUI);
        tutorialText.text = te.description;
    }

    public void DismissTutorial() {
        toggleMenu.ToggleWithoutInput(tutorialUI);
        tutorialText.text = "";
        CompleteTutorial(currentTutorialEntry);
        if (tutorialQueue.Count > 0) {
            StartCoroutine(WaitAndHandleNextEntry());
        } else {
            processingQueue = false;
        }
    }

    public void ShowLastMessageAgain() {
        DisplayTutorial(GetTutorialEntry(GetTutorialEntry("LastTutorial").description));
    }

    IEnumerator WaitAndHandleNextEntry() {
        yield return new WaitForSeconds(1);
        HandleNextEntry();
    }

    public void CompleteTutorial(TutorialEntry te) {
        if (te != null) {
            te.completed = true;
            string json = ConvertToJson();
            fh.Save(FileHandler.FileType.Tutorial, json);
        } else {
            Debug.LogError("Tutorial entry not found");
        }
    }

    private TutorialEntry GetTutorialEntry(string key) {
        return Array.Find(tutorialEntries, tutorialEntry => tutorialEntry.name == key);
    }
}

[Serializable]
public class TutorialEntry {
    public string name;
    public string description;
    public bool completed;
}