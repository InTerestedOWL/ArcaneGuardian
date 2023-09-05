using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Combat;
using AG.MovementCore;
using System.Linq;

namespace AG.Control
{
    public class AiChasePlayerState : AiState
    {
        float timer = 0.0f;

        public AiStateId GetId()
        {
            return AiStateId.AiChasePlayer;
        }

        public void Enter(StateMachineController controller)
        {
        }

        public void Update(StateMachineController controller)
        {   
            if(!controller.enabled){
                return;
            }

            //TODO: Fix knockback
            if (controller.combatTarget.currentHealth > 0)
            {
                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    float closest = float.MaxValue;
                    GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Player")
                        .Concat(GameObject.FindGameObjectsWithTag("POI"))
                        .Concat(GameObject.FindGameObjectsWithTag("Turret"))
                        .ToArray();
                    GameObject closestTarget = null;
                    foreach(GameObject target in allTargets) {
                        float distance = Vector3.Distance(target.transform.position, controller.movement.navMeshAgent.destination);
                        if(distance < closest){
                            closest = distance;
                            closestTarget = target;
                        }
                    }

                    if (Vector3.Distance(closestTarget.transform.position, controller.movement.navMeshAgent.destination) > controller.movement.navMeshAgent.stoppingDistance)
                    {
                        controller.movement.DoMovement(closestTarget.transform.position);
                    }
                    else {
                        //TODO: Lookat player before attacking
                        controller.HandleCombat(closestTarget);
                    }

                    timer = controller.config.movementUpdateTime;
                }
            }
        }

        public void Exit(StateMachineController controller)
        {
        }
    }
}
