// Based on https://www.udemy.com/course/unityrpg/

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using AG.MovementCore;
using AG.Combat;
using AG.Weapons;

namespace AG.Control {
    public class AiController : StateMachineController {

        void Start() {
            player = GameObject.Find("Player");
            movement =  GetComponent<Movement>();
            combat = GetComponent<BasicCombat>();
            combatTarget = GetComponent<CombatTarget>();
            weapon = GetComponentInChildren<Weapon>();

            // movement.navMeshAgent.stoppingDistance = config.attackRange - 0.1f;
            
            //Ai State Machine
            Debug.Log("AiController: " + this.gameObject.name);
            Debug.Log(this);
            stateMachine = new AiStateMachine(this);
            stateMachine.RegisterState(new AiChasePlayerState());
            stateMachine.RegisterState(new AiDeathState()); //TODO: brauchen wir einen DeathState? -> wird in combatTarget umgesetzt
            stateMachine.RegisterState(new AiIdleState());
            stateMachine.RegisterState(new AttackPlayerState());
            stateMachine.ChangeState(initialState);
        }
    }
}
