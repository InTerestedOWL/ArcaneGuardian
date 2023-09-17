using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : MonoBehaviour {
    [SerializeField]
    GameObject endScreen;

    public void TriggerGameOver() {
        endScreen.SetActive(true);
    }
}
