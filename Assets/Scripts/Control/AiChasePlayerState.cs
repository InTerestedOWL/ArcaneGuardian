using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Combat;
using AG.MovementCore;

namespace AG.Control
{
    public class AiChasePlayerState : AiState
    {
        float timer = 0.0f;

        public AiStateId GetId()
        {
            return AiStateId.ChasePlayer;
        }

        public void Enter(AiController controller)
        {
        }

        public void Update(AiController controller)
        {   
            if(!controller.enabled){
                return;
            }
            //TODO: Get closest "player" target and chase (Player, POI, Towers, ...)

            //TODO: Fix knockback
            if (controller.combatTarget.currentHealth > 0)
            {
                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    if (Vector3.Distance(controller.player.transform.position, controller.movement.navMeshAgent.destination) > controller.movement.navMeshAgent.stoppingDistance)
                    {
                        controller.movement.DoMovement(controller.player.transform.position);
                    }
                    else {
                        controller.HandleCombat();
                    }

                    timer = controller.config.movementUpdateTime;
                }
            }
        }

        public void Exit(AiController controller)
        {
        }
    }
}
