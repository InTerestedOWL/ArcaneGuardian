using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlayer : MonoBehaviour {
    GameObject player;

    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    void Update() {
        // Get Player and attach to it
        if (player != null) {
            transform.position = player.transform.position;
        }
    }
}
