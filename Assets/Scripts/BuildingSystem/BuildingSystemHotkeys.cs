using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BuildingSystemHotkeys : MonoBehaviour
{
    [SerializeField] private InputActionReference iar;
    public GameObject buildingObject;
    [SerializeField] private TMP_Text buildingCostText;
    
    [SerializeField] private TMP_Text infoBoxBuildingName;
    [SerializeField] private TMP_Text infoBoxBuildingDesc;

    [SerializeField] private TMP_Text infoBoxBuildingCost;
    
    // Start is called before the first frame update
    void Start()
    {
        iar.action.started += TriggerHotkey;
        if(buildingObject != null){
            PlaceableObject g = buildingObject.gameObject.GetComponent<PlaceableObject>();
            buildingCostText.text = g.getPrice().ToString();
            infoBoxBuildingName.text = g.getBuildingName();
            infoBoxBuildingDesc.text = g.getBuildingDesc();
            infoBoxBuildingCost.text = g.getPrice().ToString()+" Gold";
        }else{
            Debug.Log("ET OFF");
            EventTrigger et = this.gameObject.GetComponent<EventTrigger>();
            et.enabled = false;
        }
    }
    public TMP_Text getBuildingCostTextRef(){
        return buildingCostText;
    }
    private void TriggerHotkey(InputAction.CallbackContext context) {
        GameObject Grid = GameObject.Find("Grid");
        if(buildingObject != null){
            BuildingSystem bs = Grid.GetComponent<BuildingSystem>();
            bs.startBuilding(buildingObject);   
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
