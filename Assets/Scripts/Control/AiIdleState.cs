using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Control
{
    public class AiIdleState : AiState
    {
        public AiStateId GetId()
        {
            return AiStateId.Idle;
        }

        public void Enter(AiController controller)
        {
        }

        public void Update(AiController controller)
        {
            //TODO: Random movement

            if (controller != null && controller.player != null) {
                Vector3 playerDirection = controller.player.transform.position - controller.transform.position;

                if(playerDirection.magnitude > controller.config.maxSightDistance) {
                    return;
                }

                Vector3 controllerDirection = controller.transform.forward;
                controllerDirection.Normalize();

                float dotProduct = Vector3.Dot(playerDirection, controllerDirection);
                if(dotProduct > 0.0f) {
                    controller.stateMachine.ChangeState(AiStateId.ChasePlayer);
                }
            }
        }

        public void Exit(AiController controller)
        {
        }
    }
}
