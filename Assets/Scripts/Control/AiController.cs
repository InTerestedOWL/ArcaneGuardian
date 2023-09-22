// Based on https://www.udemy.com/course/unityrpg/

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using AG.MovementCore;
using AG.Combat;
using AG.Weapons;

namespace AG.Control {
    public class AiController : StateMachineController {

        protected new void Start() {
            base.Start();
                        
            //Ai State Machine
            stateMachine = new AiStateMachine(this);
            stateMachine.RegisterState(new AiChasePlayerState());
            stateMachine.RegisterState(new AiDeathState()); //TODO: brauchen wir einen DeathState? -> wird in combatTarget umgesetzt
            stateMachine.RegisterState(new AiIdleState());
            stateMachine.RegisterState(new AiAttackState());
            stateMachine.ChangeState(initialState);
        }
    }
}
