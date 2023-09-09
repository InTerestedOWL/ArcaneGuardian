using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.MovementCore;
using AG.Combat;
using AG.Weapons;

namespace AG.Control
{
    public class StateMachineController : MonoBehaviour
    {
        public AiStateMachine stateMachine;
        public AiStateId initialState;
        public AiControllerConfig config;
        [HideInInspector]
        public Weapon weapon;
        [HideInInspector]
        public GameObject attackTarget;

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
                    attackTarget = target;
                    combat.Attack();
                }
            }
        }

        //Animation Event für z.B. Bow Animation
        public void UseWeapon() {
            weapon.Use(attackTarget);
        }
    }
}
