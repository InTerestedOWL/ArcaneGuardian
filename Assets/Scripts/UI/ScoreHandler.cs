using System;
using System.Collections;
using System.Collections.Generic;
using AG.Control;
using AG.Files;
using AG.UI;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour {
    [SerializeField]
    private GameObject nameContainer;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_InputField playerName;
    [SerializeField]
    private PlayerResources playerResources;
    [SerializeField]
    ToggleMenu toggleMenu;
    [SerializeField]
    TextAsset defaultScore;
    [SerializeField]
    SceneHandler sceneHandler;
    private FileHandler fh;
    private ScoreEntry[] scoreEntries;
    private ScoreEntry playerScore;
    private GlobalAudioSystem globalAudioSystem;

    void Awake() {
        globalAudioSystem = GameObject.Find("Main Camera").GetComponent<GlobalAudioSystem>();
        fh = ScriptableObject.CreateInstance<FileHandler>();
        string loadedFileData = fh.Load(FileHandler.FileType.Score);
        if (loadedFileData != null) {
            scoreEntries = JsonHelper.FromJson<ScoreEntry>(loadedFileData);
        } else {
            Debug.Log("No Score file found, using default.");
            LoadDefaultScore();
        }
    }

    private void LoadDefaultScore() {
        scoreEntries = JsonHelper.FromJson<ScoreEntry>(defaultScore.text);
        string json = ConvertToJson();
        fh.Save(FileHandler.FileType.Score, json);
    }

    // Can't be set in FileHandler, as it won't work there
    private string ConvertToJson() {
        string json = JsonHelper.ToJson(scoreEntries, true);
        return json;
    }

    private bool IsScoreInScoreboard(int score) {
        Debug.Log(scoreEntries);
        if (scoreEntries.Length > 0) {
            if (scoreEntries[scoreEntries.Length - 1].score < score) {
                return true;
            }
        }
        return false;
    }

    // Sorts the scoreEntries by score
    private void SortScoreEntries() {
        Array.Sort(scoreEntries, delegate (ScoreEntry x, ScoreEntry y) {
            return y.score.CompareTo(x.score);
        });
    }

    public void HandleScoreBoard() {
        if (IsScoreInScoreboard(playerScore.score)) {
            if (!(playerName == null || playerName.text == "")) {
                playerScore.name = playerName.text;
            }
            AddScoreEntry();
        }
        sceneHandler.ChangeScene();
    }

    public void AddScoreEntry() {
        if (playerScore.name != "" && scoreEntries.Length > 0) {
            scoreEntries[scoreEntries.Length - 1] = playerScore;
            SortScoreEntries();
            string json = ConvertToJson();
            fh.Save(FileHandler.FileType.Score, json);
        }
    }

    private void OnEnable() {
        Time.timeScale = 0;
        AudioListener.pause = true;
        if (globalAudioSystem != null) {
            globalAudioSystem.PlayGameOverSound();
        }
        GameObject playerObj = GameObject.Find("Player");
        ActionMapHandler actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
        actionMapHandler.ChangeToActionMap("UI");
        toggleMenu.preventInput = false;
        playerScore = new ScoreEntry() {
            name = "Arcane Guardian",
            score = playerResources.getScore()
        };
        scoreText.text = playerScore.score.ToString();
        if (IsScoreInScoreboard(playerScore.score)) {
            nameContainer.SetActive(true);
        } else {
            nameContainer.SetActive(false);
        }
    }
}

[Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
}