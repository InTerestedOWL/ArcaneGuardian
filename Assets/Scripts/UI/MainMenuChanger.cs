using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuChanger : MonoBehaviour {
    [SerializeField] 
    GameObject mainMenu;
    [SerializeField]
    GameObject optionsMenu;
    [SerializeField]
    GameObject scoreboard;

    public void ShowMainMenu() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        scoreboard.SetActive(false);
    }

    public void ShowOptionsMenu() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        scoreboard.SetActive(false);
    }

    public void ShowScoreboard() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        scoreboard.SetActive(true);
    }
}
