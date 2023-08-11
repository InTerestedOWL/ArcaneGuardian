using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.MovementCore;
using AG.Combat;

namespace AG.Control
{
    public class StateMachineController : MonoBehaviour
    {
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

        // Update is called once per frame
        void Update() {
            stateMachine.Update();
        }

        public void HandleCombat(GameObject target) {
            if(Vector3.Distance(this.transform.position, target.transform.position) < config.attackRange) {
                if(!combat.IsAttacking()){
                    combat.Attack();
                }
            }
        }
    }
}
