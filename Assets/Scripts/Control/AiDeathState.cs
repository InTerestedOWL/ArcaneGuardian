using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Control
{
    public class AiDeathState : AiState
    {
        public AiStateId GetId()
        {
            return AiStateId.AiDeath;
        }

        public void Enter(StateMachineController controller)
        {
            controller.gameObject.GetComponent<Collider>().enabled = false;
        }

        public void Update(StateMachineController controller)
        {
        }

        public void Exit(StateMachineController controller)
        {
        }
    }
}
