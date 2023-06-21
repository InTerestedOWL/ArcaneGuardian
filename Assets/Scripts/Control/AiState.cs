using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AG.Control {
    public enum AiStateId {
        ChasePlayer,
        Death,
        Idle,
        AttackPlayer
    }

    public interface AiState
    {
        AiStateId GetId();
        void Enter(AiController controller);
        void Update(AiController controller);
        void Exit(AiController controller);
    }    
}

