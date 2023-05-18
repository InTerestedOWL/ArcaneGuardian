using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerMovement : MonoBehaviour
{
    [SerializeField] Transform targetToFollow;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = targetToFollow.position;
    }
}
