using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;
    
    // Start is called before the first frame update
    private void OnMouseDown(){
        //offset = transform.position - BuildingSystem.GetMouseWorldPosition();  
    }
    void Start()
    {
         offset = transform.position - BuildingSystem.current.GetMouseWorldPosition();    
    }

    private void OnMouseDrag(){
        //Vector3 pos = BuildingSystem.GetMouseWorldPosition()+offset;
        //transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = BuildingSystem.current.GetMouseWorldPosition()+offset;
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
    }
}
