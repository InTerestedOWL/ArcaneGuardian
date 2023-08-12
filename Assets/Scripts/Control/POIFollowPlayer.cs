using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Combat;
using AG.MovementCore;

namespace AG.Control
{
    public class POIFollowPlayerState : AiState
    {
        float timer = 0.0f;

        public AiStateId GetId()
        {
            return AiStateId.POIFollowPlayer;
        }

        public void Enter(StateMachineController controller)
        {
            //TODO: Add POI to Bulding ActionBar
        }

        public void Update(StateMachineController controller)
        {   
            if(!controller.enabled){
                return;
            }

            if (controller.combatTarget.currentHealth > 0)
            {
                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    if (Vector3.Distance(controller.player.transform.position, controller.movement.navMeshAgent.destination) > controller.movement.navMeshAgent.stoppingDistance)
                    {
                        controller.movement.DoMovement(controller.player.transform.position);
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
