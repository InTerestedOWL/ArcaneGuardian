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

        //TODO: Problem mit CombatTarget -> CombatTarget handlet wenn eine einheit stirbt
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
