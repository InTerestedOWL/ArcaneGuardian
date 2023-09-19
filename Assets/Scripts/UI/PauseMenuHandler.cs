using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour {
    public static bool gameIsPaused = false;
    [SerializeField]
    private GlobalAudioSystem globalAudioSystem;

    void OnEnable() {
        globalAudioSystem.PlayUIPopupOpenSound();
        TutorialHandler.AddTutorialToShow("Pause", "Tutorial");
        Time.timeScale = 0;
        AudioListener.pause = true;
        gameIsPaused = true;
    }

    void OnDisable() {
        globalAudioSystem.PlayUIPopupCloseSound();
        if (!TutorialHandler.tutorialActive) {
            Time.timeScale = 1;
        }
        AudioListener.pause = false;
        gameIsPaused = false;
        TutorialHandler.AddTutorialToShow("Goal", "Pause");
        TutorialHandler.AddTutorialToShow("Movement", "Pause");
    }
}
