using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - BuildingSystem.GetMouseWorldPosition();     
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = BuildingSystem.GetMouseWorldPosition()+offset;
        transform.position = BuildingSystem.current.SnapCoordinateToGrid(pos);
    }
}
