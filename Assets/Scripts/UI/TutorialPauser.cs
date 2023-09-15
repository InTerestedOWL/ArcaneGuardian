using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPauser : MonoBehaviour {
    void OnEnable() {
        Time.timeScale = 0;
        TutorialHandler.tutorialActive = true;
    }

    void OnDisable() {
        if (!PauseMenuHandler.gameIsPaused) {
            Time.timeScale = 1;
        }
        TutorialHandler.tutorialActive = false;
    }
}
