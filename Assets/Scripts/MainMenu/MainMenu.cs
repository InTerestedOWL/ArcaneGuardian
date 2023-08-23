using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool isStart;
    public bool isQuit;
    public bool isSettings;
    public GameObject settingsMenu;
    public GameObject mainMenu;
    
    void OnMouseUp() {
        if(isStart)
        {
            Application.LoadLevel(1);
        }
        if (isQuit)
        {
            Application.Quit();
        }

        if (isSettings)
        {
            settingsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
    } 
}
