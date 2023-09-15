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
            try {
                Enum ftEnum = ft;
                Debug.Log($"Saved to {Application.persistentDataPath}/{ftEnum}.json");
                File.WriteAllText($"{Application.persistentDataPath}/{ftEnum}.json", jsonString);
            } catch (Exception e) {
                Debug.LogError(e);
            }
        }

        public string Load(FileType ft) {
            try {
                Enum ftEnum = ft;
                string savedString = File.ReadAllText($"{Application.persistentDataPath}/{ftEnum}.json");
                if (savedString != null) {
                    return savedString;
                } else {
                    Debug.LogError("No file found");
                    return null;
                }
            } catch (Exception e) {
                Debug.LogError(e);
                return null;
            }
        }
    }
}