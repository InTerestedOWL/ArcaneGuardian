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

        public void Save(FileType ft, string jsonString) {
            Debug.Log("Save");

            Enum ftEnum = ft;

            Debug.Log(jsonString);

            Debug.Log($"Saved to {Application.persistentDataPath}/{ftEnum}.json");

            File.WriteAllText($"{Application.persistentDataPath}/{ftEnum}.json", jsonString);
        }

        public void Load() {
            
        }
    }
}