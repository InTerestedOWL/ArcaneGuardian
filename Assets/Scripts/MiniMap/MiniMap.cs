using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField]
    GameObject FollowTarget;

    [SerializeField] private int heightOfIcon = 50;

    // Update is called once per frame
    void Update()
    {   
        //Update Position of Camera without Rotating it
        //We want our MiniMap to always be north facing
        this.transform.position = new Vector3(FollowTarget.transform.position.x, heightOfIcon + FollowTarget.transform.position.y, FollowTarget.transform.position.z);
    }
}
