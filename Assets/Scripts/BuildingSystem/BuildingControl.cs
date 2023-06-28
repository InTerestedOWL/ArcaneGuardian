using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class BuildingControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private InputActionReference iar;
    void Start()
    {
        iar.action.started += TriggerHotkey;
    }

    // Update is called once per frame
    private void TriggerHotkey(InputAction.CallbackContext context) {
        GameObject Grid = GameObject.Find("Grid");
        BuildingSystem bs = Grid.GetComponent<BuildingSystem>();
        bs.stopBuilding();
    }   
    void Update()
    {
        
    }
}
