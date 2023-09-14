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
    private FileHandler fh;
    private TutorialEntry[] tutorialEntries;
    private Queue<TutorialEntry> tutorialQueue;
    private bool processingQueue = false;
    private TutorialEntry currentTutorialEntry;

    void Start() {
        tutorialQueue = new Queue<TutorialEntry>();
        fh = ScriptableObject.CreateInstance<FileHandler>();
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
        AddTutorialToShow("Movement");
        AddTutorialToShow("Goal");
        AddTutorialToShow("Crystal");
        AddTutorialToShow("BuildingMode");
    }

    private void LoadDefaultTutorial() {
        tutorialEntries = JsonHelper.FromJson<TutorialEntry>(defaultTutorial.text);
        string json = ConvertToJson();
        fh.Save(FileHandler.FileType.Tutorial, json);
    }

    public void ResetTutorial() {
        LoadDefaultTutorial();
        InitTutorials();
    }

    // Can't be set in FileHandler, as it won't work there
    private string ConvertToJson() {
        string json = JsonHelper.ToJson(tutorialEntries, true);
        return json;
    }

    public void AddTutorialToShow(string key) {
        TutorialEntry te = GetTutorialEntry(key);
        if (te != null) {
            if (!te.completed) {
                tutorialQueue.Enqueue(te);
                ShowTutorials();
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

    IEnumerator WaitAndHandleNextEntry() {
        Debug.Log("Wait for new tutorial entry");
        yield return new WaitForSeconds(1);
        HandleNextEntry();
    }

    public void CompleteTutorial(TutorialEntry te) {
        if (te != null) {
            te.completed = true;
            string json = ConvertToJson();
            Debug.Log(json);
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