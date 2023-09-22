using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Control;

namespace AG.Control {
    public class AiStateMachine
    {
        public AiState[] states;
        public StateMachineController controller;
        public AiStateId currentState;

        public AiStateMachine(StateMachineController controller)
        {
            this.controller = controller;
            int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
            states = new AiState[numStates];
        }

        public void RegisterState(AiState state){
            int index = (int)state.GetId();
            states[index] = state;
        }

        public AiState GetState(AiStateId stateId){
            return states[(int)stateId];
        }

        public void Update(){
            GetState(currentState)?.Update(controller);
        }

        public void ChangeState(AiStateId newState){
            GetState(currentState)?.Exit(controller);
            currentState = newState;
            GetState(currentState)?.Enter(controller);
        }
    }
}
