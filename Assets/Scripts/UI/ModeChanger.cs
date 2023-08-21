using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AG.Control;

public class ModeChanger : MonoBehaviour {
    private ActionMapHandler actionMapHandler = null;

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
            if (actionMapHandler == null) {
                GameObject playerObj = GameObject.Find("Player");
                actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
            }
            GameObject grid = GameObject.Find("Tilemap");
            if (!actionBarSkills.activeSelf) {
                actionMapHandler.ChangeToActionMap("Player", false);               
                grid.GetComponent<Renderer>().enabled = false;               
            } else {
                actionMapHandler.ChangeToActionMap("Player (Building)", false);                
                grid.GetComponent<Renderer>().enabled = true;  
            }
            actionBarSkills.SetActive(!actionBarSkills.activeSelf);
            actionBarBuilding.SetActive(!actionBarBuilding.activeSelf);
        }
    }
}
