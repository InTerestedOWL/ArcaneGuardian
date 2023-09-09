using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG.Combat;
using AG.MovementCore;
using System.Linq;
using System.IO;
using UnityEngine.AI;
using System;
using AG.Skills.Targeting;
using Unity.VisualScripting;

namespace AG.Control
{
    public class AiChasePlayerState : AiState
    {
        float timer = 0.0f;

        public AiStateId GetId()
        {
            return AiStateId.AiChasePlayer;
        }

        public void Enter(StateMachineController controller)
        {
        }

        public void Update(StateMachineController controller)
        {
            if (!controller.enabled)
            {
                return;
            }

            //TODO: Fix knockback
            if (controller.combatTarget.currentHealth > 0)
            {
                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    // float closest = float.MaxValue;
                    // GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Player")
                    //     .Concat(GameObject.FindGameObjectsWithTag("POI"))
                    //     .Concat(GameObject.FindGameObjectsWithTag("Turret"))
                    //     .ToArray();
                    // GameObject closestTarget = null;
                    // foreach (GameObject target in allTargets)
                    // {
                    //     float distance = Vector3.Distance(target.transform.position, controller.movement.navMeshAgent.destination);
                    //     if (distance < closest)
                    //     {
                    //         closest = distance;
                    //         closestTarget = target;
                    //     }
                    // }

                    GameObject target = GetClosestAttackableTarget(controller);


                    //Wenn kein Target gefunden wurde, dann Idle
                    if(target == null){
                        // controller.stateMachine.ChangeState(AiStateId.AiIdle);
                        return;
                    }

                    if (Vector3.Distance(target.transform.position, controller.transform.position) > controller.movement.navMeshAgent.stoppingDistance)
                    {
                        if(!controller.combat.IsAttacking()){
                            controller.movement.DoMovement(target.transform.position);
                        }
                    }
                    else
                    {
                        //TODO: Lookat smooth player before attacking
                        controller.transform.LookAt(target.transform);
                        // controller.StartCoroutine(RotateToTarget(closestTarget, controller));
                        controller.HandleCombat(target);
                    }

                    timer = controller.config.movementUpdateTime;
                }
            }
        }

        public void Exit(StateMachineController controller)
        {
        }

        // private IEnumerator RotateToTarget(GameObject target, StateMachineController controller)
        // {
        //     Vector3 direction = (target.transform.position - controller.transform.position).normalized;
        //     Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        //     float rotationThreshold = 0.1f; // Eine Schwelle für die Rotation

        //     while (Quaternion.Angle(target.transform.rotation, lookRotation) > rotationThreshold)
        //     {
        //         target.transform.rotation = Quaternion.Slerp(target.transform.rotation, lookRotation, Time.deltaTime * 10f);
        //         yield return null;
        //     }
        // }

        private GameObject GetClosestAttackableTarget(StateMachineController controller) {
            float closest = float.MaxValue;
            GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Player")
                .Concat(GameObject.FindGameObjectsWithTag("POI"))
                .Where(g => g.GetComponent<CombatTarget>().currentHealth > 0)
                .ToArray();

            GameObject[] allTurrets = GameObject.FindGameObjectsWithTag("Turret")
                .Where(g => g.GetComponent<CombatTarget>().currentHealth > 0)
                .ToArray();

            GameObject closestTarget = null;
            foreach (GameObject target in allTargets)
            {
                float distance = Vector3.Distance(target.transform.position, controller.movement.navMeshAgent.destination);
                if (distance < closest)
                {
                    NavMeshPath path = new NavMeshPath();
                    controller.movement.navMeshAgent.CalculatePath(target.transform.position, path);
                    if(path.status == NavMeshPathStatus.PathComplete){
                        closest = distance;
                        closestTarget = target;
                    }
                }
            }

            if(closestTarget == null) {
               foreach (GameObject target in allTurrets)
                {
                    float distance = Vector3.Distance(target.transform.position, controller.movement.navMeshAgent.destination);
                    if (distance < closest)
                    {
                        closest = distance;
                        closestTarget = target;
                    }
                } 
            }

            return closestTarget;
        }
    }
}
