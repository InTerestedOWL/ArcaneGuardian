using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {
    [SerializeField]
    private string sceneName;
    private bool changeSceneTriggered = false;

    void Start() {
        StartCoroutine(PreloadScene(sceneName));
    }

    IEnumerator PreloadScene(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone) {
            if (asyncLoad.progress >= 0.9f && changeSceneTriggered) {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ChangeScene() {
        changeSceneTriggered = true;
    }

    public void CloseGame() {
        Application.Quit();
    }
}
