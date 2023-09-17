using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingHandler : MonoBehaviour {
    [SerializeField]
    public Slider loadingBar;
    private static int loadingPercentage = 0;
    private static float loadingBarValue = 0;
    [SerializeField]
    private TutorialHandler tutorialHandler;
    public static bool loading = true;

    public static void SetLoadingPercentage(int percentage) {
        loadingPercentage = percentage;
        loadingBarValue = loadingPercentage / 100f;
    }

    public static void AddLoadingPercentage(int percentage) {
        loadingPercentage += percentage;
        loadingBarValue = loadingPercentage / 100f;
    }

    private void Update() {
        loadingBar.value = loadingBarValue;
        if (loadingPercentage > 100) {
            loading = false;
            tutorialHandler.ShowTutorials();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        loading = true;
        AudioListener.pause = true;
        SetLoadingPercentage(0);
    }

    private void OnDisable() {
        AudioListener.pause = false;
    }
}
