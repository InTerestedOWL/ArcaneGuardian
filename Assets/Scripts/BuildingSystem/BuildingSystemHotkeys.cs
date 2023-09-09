using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class BuildingSystemHotkeys : MonoBehaviour
{
    [SerializeField] private InputActionReference iar;
    public GameObject buildingObject;
    [SerializeField] private TMP_Text buildingCostText;
    
    
    // Start is called before the first frame update
    void Start()
    {
        iar.action.started += TriggerHotkey;
        if(buildingObject != null){
            buildingCostText.text = buildingObject.gameObject.GetComponent<PlaceableObject>().getPrice().ToString();
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
