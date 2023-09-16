using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingHandler : MonoBehaviour {
    [SerializeField]
    public GameObject loadingBar;
    private static int loadingPercentage = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public static void SetLoadingPercentage(int percentage) {
        loadingPercentage = percentage;
    }

    public static void AddLoadingPercentage(int percentage) {
        loadingPercentage += percentage;
    }

    private void OnEnable() {
        AudioListener.pause = true;
        SetLoadingPercentage(0);
    }

    private void OnDisable() {
        AudioListener.pause = false;
    }
}
