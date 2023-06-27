using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeChanger : MonoBehaviour {
    [SerializeField]
    public List<InputActionReference> modeChangeActions = null;
    [SerializeField]
    GameObject actionBarSkills = null;
    [SerializeField]
    GameObject actionBarBuilding = null;

    void Start() {
        foreach(InputActionReference modeChangeAction in modeChangeActions) {
            modeChangeAction.action.performed += ctx => ToggleMode();
        }
    }

    void ToggleMode() {
        if (actionBarSkills != null && actionBarBuilding != null) {
            actionBarSkills.SetActive(!actionBarSkills.activeSelf);
            actionBarBuilding.SetActive(!actionBarBuilding.activeSelf);
        }
    }
}
