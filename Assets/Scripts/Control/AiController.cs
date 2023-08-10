// Based on https://www.udemy.com/course/unityrpg/

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using AG.MovementCore;
using AG.Combat;

namespace AG.Control {
    public class AiController : MonoBehaviour {
        public AiStateMachine stateMachine;
        public AiStateId initialState;
        public AiControllerConfig config;

        [HideInInspector]
        public GameObject player = null;

        [HideInInspector]
        public Movement movement = null;

        [HideInInspector]
        public BasicCombat combat = null;

        [HideInInspector]
        public CombatTarget combatTarget = null;


        void Start() {
            player = GameObject.Find("Player");
            movement =  GetComponent<Movement>();
            combat = GetComponent<BasicCombat>();
            combatTarget = GetComponent<CombatTarget>();
            
            //Ai State Machine
            stateMachine = new AiStateMachine(this);
            stateMachine.RegisterState(new AiChasePlayerState());
            stateMachine.RegisterState(new AiDeathState()); //TODO: brauchen wir einen DeathState? -> wird in combatTarget umgesetzt
            stateMachine.RegisterState(new AiIdleState());
            stateMachine.RegisterState(new AttackPlayerState());
            stateMachine.ChangeState(initialState);
        }

        // Update is called once per frame
        void Update() {
            stateMachine.Update();
        }
        
        public void HandleCombat() {
            if(Vector3.Distance(this.transform.position, player.transform.position) < config.attackRange) {
                if(!combat.IsAttacking()){
                    combat.Attack();
                }
            }
        }
    }
}
