using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Control;

namespace AG.Control {
    public class POIStateMachine
    {
        public POIState[] states;
        public POIController controller;
        public POIStateId currentState;

        public POIStateMachine(POIController controller)
        {
            this.controller = controller;
            int numStates = System.Enum.GetNames(typeof(POIStateId)).Length;
            states = new POIState[numStates];
        }

        public void RegisterState(POIState state){
            int index = (int)state.GetId();
            states[index] = state;
        }

        public POIState GetState(POIStateId stateId){
            return states[(int)stateId];
        }

        public void Update(){
            GetState(currentState)?.Update(controller);
        }

        public void ChangeState(POIStateId newState){
            GetState(currentState)?.Exit(controller);
            currentState = newState;
            GetState(currentState)?.Enter(controller);
        }
    }
}
