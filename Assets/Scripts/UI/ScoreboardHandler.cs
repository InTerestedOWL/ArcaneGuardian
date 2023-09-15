using AG.Files;
using UnityEngine;

public class ScoreboardHandler : MonoBehaviour {
    [SerializeField]
    TextAsset defaultScore;
    [SerializeField]
    GameObject scoreEntryPrefab;
    [SerializeField]
    GameObject container;
    private FileHandler fh;
    private ScoreEntry[] scoreEntries;

    void Awake() {
        fh = ScriptableObject.CreateInstance<FileHandler>();
        string loadedFileData = fh.Load(FileHandler.FileType.Score);
        if (loadedFileData != null)
        {
            scoreEntries = JsonHelper.FromJson<ScoreEntry>(loadedFileData);
        }
        else
        {
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

    private void OnEnable() {
        foreach (ScoreEntry scoreEntry in scoreEntries) {
            GameObject entry = Instantiate(scoreEntryPrefab, container.transform);
            entry.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = scoreEntry.name;
            entry.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = scoreEntry.score.ToString();
        }
    }

    private void OnDisable() {
        foreach (Transform child in container.transform) {
            Destroy(child.gameObject);
        }
    }
}
