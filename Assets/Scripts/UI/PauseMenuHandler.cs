using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour {
    void OnEnable() {
        TutorialHandler.AddTutorialToShow("Pause", "Tutorial");
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    void OnDisable() {
        TutorialHandler.AddTutorialToShow("Goal", "Pause");
        TutorialHandler.AddTutorialToShow("Movement", "Pause");
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}
