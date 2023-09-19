using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuChanger : MonoBehaviour {
    [SerializeField] 
    GameObject mainMenu;
    [SerializeField]
    GameObject optionsMenu;
    [SerializeField]
    SettingsHandler settingsHandler;
    [SerializeField]
    GameObject scoreboard;
    [SerializeField]
    GameObject credits;

    void Start() {
        if (settingsHandler != null) {
            settingsHandler.InitSettings();
        }
    }

    public void ShowMainMenu() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        scoreboard.SetActive(false);
        credits.SetActive(false);
    }

    public void AfterLoadingActive() {
        LoadingHandler.SetLoadingPercentage(110);
    }

    public void ShowOptionsMenu() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        scoreboard.SetActive(false);
        credits.SetActive(false);
    }

    public void ShowScoreboard() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        scoreboard.SetActive(true);
        credits.SetActive(false);
    }

    public void ShowCredits() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        scoreboard.SetActive(false);
        credits.SetActive(true);
    }

    private void OnEnable() {
        LoadingHandler.SetLoadingPercentage(110);
    }
}
