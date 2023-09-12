using System.IO;
using UnityEngine;
using AG.Files;
using System;

public class TutorialHandler : MonoBehaviour {
    private FileHandler fh;

    void Start() {
        fh = ScriptableObject.CreateInstance<FileHandler>();
    }

    public void ResetTutorial() {
        TutorialEntry te = new("Test", false);
        fh.Save(FileHandler.FileType.Tutorial, te);
    }
}

public class TutorialEntry {
    String name;
    bool completed;

    public TutorialEntry(string name, bool completed) {
        this.name = name;
        this.completed = completed;
    }
}