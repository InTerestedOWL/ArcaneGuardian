using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AG.Control {
    public enum AiStateId {
        AiChasePlayer,
        AiDeath,
        AiIdle,
        AiAttackPlayer,
        POIIdle,
        POIFollowPlayer
    }

    public interface AiState
    {
        AiStateId GetId();
        void Enter(StateMachineController controller);
        void Update(StateMachineController controller);
        void Exit(StateMachineController controller);
    }    
}

