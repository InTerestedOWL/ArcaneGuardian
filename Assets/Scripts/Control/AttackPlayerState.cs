using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Control {

    public class AttackPlayerState : AiState
    {
        public AiStateId GetId()
        {
            return AiStateId.AttackPlayer;
        }

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
