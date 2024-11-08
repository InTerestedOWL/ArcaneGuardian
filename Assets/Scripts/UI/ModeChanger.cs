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
            
            if (!actionBarSkills.activeSelf) {
                actionMapHandler.ChangeToActionMap("Player", false, true);               
                GameObject.Find("Grid").GetComponent<BuildingSystem>().decalProjector.gameObject.SetActive(false);
                TutorialHandler.AddTutorialToShow("Talents", "MoreBuildingInfo");
            } else {
                actionMapHandler.ChangeToActionMap("Player (Building)", false, true);                
                GameObject.Find("Grid").GetComponent<BuildingSystem>().decalProjector.gameObject.SetActive(true);
                TutorialHandler.AddTutorialToShow("BuildingMode", "Crystal");
            }
            actionBarSkills.SetActive(!actionBarSkills.activeSelf);
            actionBarBuilding.SetActive(!actionBarBuilding.activeSelf);
        }
    }
}
