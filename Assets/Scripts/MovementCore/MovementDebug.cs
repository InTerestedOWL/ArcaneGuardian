using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementDebug : MonoBehaviour
{
    public bool velocity;
    public bool desiredVelocity;
    public bool path;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnDrawGizmos()
    {   
        //TODO: Wirft einen Fehler, wenn das Spiel nicht gestartet ist -> Warum?
        if(velocity) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position, this.transform.position + agent.velocity);
        }

        if(desiredVelocity) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, this.transform.position + agent.desiredVelocity);
        }

        if(path){
            Gizmos.color = Color.black;
            Vector3 prevCorner = this.transform.position;
            foreach(var corner in agent.path.corners){
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }
    }
}
