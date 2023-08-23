using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public bool isBack;
    public GameObject settingsMenu;
    public GameObject mainMenu;
    
    void OnMouseUp() {
        if (isBack)
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    } 
}
