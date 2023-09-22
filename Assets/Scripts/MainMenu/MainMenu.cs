using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool isStart;
    public bool isQuit;
    public bool isSettings;
    [SerializeField]
    public GameObject sceneManager;
    public GameObject settingsMenu;
    public GameObject mainMenu;
    
    void OnMouseUp() {
        if(isStart) {
            sceneManager.GetComponent<SceneHandler>().ChangeScene();
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
