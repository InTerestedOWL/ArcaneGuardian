using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AiControllerConfig : ScriptableObject
{   
    public float attackRange = 1.5f;
    public float movementUpdateTime = 1.0f;
    public float maxSightDistance = 5.0f;
}
