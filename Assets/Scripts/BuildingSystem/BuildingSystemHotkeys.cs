using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class BuildingSystemHotkeys : MonoBehaviour
{
    [SerializeField] private InputActionReference iar;
    public GameObject buildingObject;
    
    
    // Start is called before the first frame update
    void Start()
    {
        iar.action.started += TriggerHotkey;
    }

    private void TriggerHotkey(InputAction.CallbackContext context) {
        GameObject Grid = GameObject.Find("Grid");
        BuildingSystem bs = Grid.GetComponent<BuildingSystem>();
        bs.startBuilding(buildingObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
