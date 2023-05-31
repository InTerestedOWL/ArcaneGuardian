// Based on https://www.udemy.com/course/unityrpg/

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

using AG.MovementCore;
using AG.Combat;

namespace AG.Control {
    public class AiController : MonoBehaviour {
        public float attackRange = 1.5f;
        public float movementUpdateTime = 1.0f;

        GameObject player = null;
        bool attacking = false;
        Movement movement = null;
        //TODO: Vllt schlecht das so zu machen, weil movement grundsätzlich sich um alles kümmert...
        NavMeshAgent navMeshAgent;
        float timer = 0.0f;
        BasicCombat combat = null;
        CombatTarget combatTarget = null;


        void Start() {
            player = GameObject.Find("Player");
            movement =  GetComponent<Movement>();
            combat = GetComponent<BasicCombat>();
            combatTarget = GetComponent<CombatTarget>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update() {
            if(combatTarget.currentHealth > 0) {
                timer -= Time.deltaTime;

                if(timer <= 0.0f) {
                    HandleCombat();
                    HandleMovement();

                    timer = movementUpdateTime;
                }
            }
        }

        // Movement for keyboard input
        private void HandleMovement() {
            //TODO: navMeshAgent.stoppingDistance könnte vllt Probleme machen
            if(Vector3.Distance(player.transform.position, navMeshAgent.destination) > navMeshAgent.stoppingDistance){
                Debug.Log("updating movement");
                movement.DoMovement(player.transform.position);
            }
        }
        
        private void HandleCombat() {
            if(Vector3.Distance(this.transform.position, player.transform.position) < attackRange) {
                combat.Attack();
            }
        }
    }
}
