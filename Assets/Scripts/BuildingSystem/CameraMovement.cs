using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private bool movingW = false;
    private bool movingA = false;
    private bool movingS = false;
    private bool movingD = false;

    private float movSpeed = 0.2f;
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)){
            movingW = true;        
        }
        else if(Input.GetKeyUp(KeyCode.W)){
            movingW = false;
        }

        if(Input.GetKeyDown(KeyCode.A)){
            movingA = true;     
        }
        else if(Input.GetKeyUp(KeyCode.A)){
            movingA = false;
        }
        
        if(Input.GetKeyDown(KeyCode.S)){
            movingS = true;       
        }
        else if(Input.GetKeyUp(KeyCode.S)){
            movingS = false;
        }

        if(Input.GetKeyDown(KeyCode.D)){
            movingD = true;
        }
        else if(Input.GetKeyUp(KeyCode.D)){
            movingD = false;
        }

        if(movingW){
            this.transform.position = this.transform.position + new Vector3(1, 0, 1)*movSpeed;
        }
        if(movingA){
            this.transform.position = this.transform.position + new Vector3(-1, 0, 1)*movSpeed;    
        }
        if(movingS){
            this.transform.position = this.transform.position + new Vector3(-1, 0, -1)*movSpeed;
        }
        if(movingD){
            this.transform.position = this.transform.position + new Vector3(1, 0, -1)*movSpeed;
        }

    }
}
