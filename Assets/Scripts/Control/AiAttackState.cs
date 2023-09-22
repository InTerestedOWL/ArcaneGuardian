using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Control {

    public class AiAttackState : AiState
    {
        public AiStateId GetId()
        {
            return AiStateId.AiAttackPlayer;
        }

        public void Enter(StateMachineController controller)
        {
        }

        public void Update(StateMachineController controller)
        {
        }

        public void Exit(StateMachineController controller)
        {
        } 
    }
}
