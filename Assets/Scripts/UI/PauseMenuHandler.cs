using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour {
    void OnEnable() {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    void OnDisable() {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}
