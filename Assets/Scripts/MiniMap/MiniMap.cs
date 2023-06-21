using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField]
    GameObject FollowTarget;

    // Update is called once per frame
    void Update()
    {   
        //Update Position of Camera without Rotating it
        //We want our MiniMap to always be north facing
        this.transform.position = new Vector3(FollowTarget.transform.position.x, this.transform.position.y, FollowTarget.transform.position.z);
    }
}
