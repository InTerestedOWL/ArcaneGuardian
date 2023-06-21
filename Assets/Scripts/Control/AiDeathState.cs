using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Control
{
    public class AiDeathState : AiState
    {
        public AiStateId GetId()
        {
            return AiStateId.Death;
        }

        //TODO: Problem mit CombatTarget -> CombatTarget handlet wenn eine einheit stirbt
        public void Enter(AiController controller)
        {
        }

        public void Update(AiController controller)
        {
        }

        public void Exit(AiController controller)
        {
        }
    }
}
