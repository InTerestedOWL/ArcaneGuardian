using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuChanger : MonoBehaviour {
    [SerializeField] 
    GameObject mainMenu;
    [SerializeField]
    GameObject optionsMenu;

    public void ShowMainMenu() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ShowOptionsMenu() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
}
