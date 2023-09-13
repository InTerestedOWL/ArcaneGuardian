// Inspired by https://www.youtube.com/watch?v=VR0mIs80Gys

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AG.Files {
    public class FileHandler : ScriptableObject {
        public enum FileType {
            Settings,
            Tutorial
        }

        public void Save(FileType ft, System.Object dataToSave) {
            Enum ftEnum = ft;

            string json = JsonUtility.ToJson(dataToSave, true);

            Debug.Log($"Saved to {Application.persistentDataPath}/{ftEnum}.json");

            File.WriteAllText($"{Application.persistentDataPath}/{ftEnum}.json", json);
        }

        public void Load() {
            
        }
    }
}