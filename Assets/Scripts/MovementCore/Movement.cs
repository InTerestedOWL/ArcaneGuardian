// Based on https://www.udemy.com/course/unityrpg/

using UnityEngine;
using UnityEngine.AI;

using AG.Control;

namespace AG.MovementCore {
    public class Movement : MonoBehaviour {
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        float startAngularSpeed;

        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            startAngularSpeed = navMeshAgent.angularSpeed;
        }
        private void Update() {
            UpdateAnimator();
        }

        /*
         * Move to target.
         */
        public void DoMovement(Vector3 newDestination) {
            navMeshAgent.SetDestination(newDestination);
        }

        private void UpdateAnimator() {
            // Get velocity for animator
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            Animator animator = GetComponent<Animator>();
            // Set Animator blend tree value to movement speed
            animator.SetFloat("walkSpeed", speed);
            if (!animator.runtimeAnimatorController.name.Equals("POI")) {
                animator.SetFloat("sideSpeed", localVelocity.x);
            }
        }

        /*
         * Cancel movement.
         */
        public void Cancel() {
            navMeshAgent.isStopped = true;
        }


        /*
         * Used by Attack Animation Event.
         */
        public void SuspendRotation() {
            navMeshAgent.angularSpeed = 0;
            // TODO: Change Slash Animation to ignore legs when attacking
        }

        /*
         * Used by Attack Animation Event.
         */
        public void ResumeRotation() {
            navMeshAgent.angularSpeed = startAngularSpeed;
        }

        public NavMeshAgent getNavMeshAgent(){
            return navMeshAgent;
        }
    }
}