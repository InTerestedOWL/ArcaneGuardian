using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingHandler : MonoBehaviour {
    private static int loadingPercentage = 0;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public static void SetLoadingPercentage(int percentage) {
        Debug.Log("Loading percentage: " + percentage);
    }

    public static void AddLoadingPercentage(int percentage) {
        loadingPercentage += percentage;
    }

    private void OnEnable() {
        SetLoadingPercentage(0);
    }
}
