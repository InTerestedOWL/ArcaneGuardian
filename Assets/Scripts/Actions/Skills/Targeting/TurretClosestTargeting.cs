// Based on https://www.udemy.com/course/rpg-shops-abilities/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AG.Combat;
using AG.Control;
using AG.Skills.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AG.Skills.Targeting {
    [CreateAssetMenu(fileName = "Turret Closest Targeting", menuName = ("Arcane Guardian/Targeting Strategy/Turret Closest Targeting"))]
    public class TurretClosestTargeting : TargetingStrategy {

        public override void DeclareTargets(SkillData data, Action callback) {
            GameObject user = data.GetUser();
            if (user) {
                data.GetTurretController().StartCoroutine(SelectClosestTarget(data, callback));
            }
        }

        private IEnumerator SelectClosestTarget(SkillData data, Action callback) {

            GameObject user = data.GetUser();

            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy").Where(t => t.GetComponent<CombatTarget>()?.currentHealth > 0).ToArray();
            GameObject closestTarget = null;

            float closest = float.MaxValue;
            foreach(GameObject target in targets) {
                float distance = Vector3.Distance(target.transform.position, user.transform.position);
                if(distance < closest){
                    closest = distance;
                    closestTarget = target;
                }
            }

            if(closestTarget != null) {
                List<GameObject> target = new List<GameObject>
                {
                    closestTarget
                };
                data.SetTargets(target);
                data.SetTargetPosition(closestTarget.transform.position);
                callback();
            }
            
            yield return null;
        }
    }
}