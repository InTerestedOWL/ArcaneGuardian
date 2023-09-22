// Based on https://www.udemy.com/course/unityrpg/

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using AG.MovementCore;
using AG.Combat;

namespace AG.Control {
    public class POIController : StateMachineController {

        protected new void Start() {
            base.Start();
            
            //Ai State Machine
            stateMachine = new AiStateMachine(this);
            stateMachine.RegisterState(new POIFollowPlayerState());
            stateMachine.RegisterState(new POIIdleState());
            stateMachine.RegisterState(new POIBuildingState());
            stateMachine.ChangeState(initialState);
        }
    }
}