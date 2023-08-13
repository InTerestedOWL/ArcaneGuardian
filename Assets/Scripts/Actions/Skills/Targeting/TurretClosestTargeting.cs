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

        [SerializeField] 
        float aoeRadius = 1f;
        [SerializeField]
        Transform telegraphPrefab = null;
        Transform telegraphInstance = null;

        public override void DeclareTargets(SkillData data, Action callback) {
            GameObject user = data.GetUser();
            if (user) {
                data.GetTurretController().StartCoroutine(SelectClosestTarget(data, callback));
            }
        }

        private IEnumerator SelectClosestTarget(SkillData data, Action callback) {

            data.SetRadius(aoeRadius);
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
                //TODO: Display Targeting while VFX is played

                data.SetTargets(GetAoETargets(closestTarget));
                data.SetTargetPosition(closestTarget.transform.position);
                callback();
            }

            yield return null;
        }

        // Selects all targets around the target
        private IEnumerable<GameObject> GetAoETargets(GameObject target) {
            RaycastHit[] hits = Physics.SphereCastAll(target.transform.position, aoeRadius, Vector3.up, 0f);
            foreach (RaycastHit hit in hits) {
                yield return hit.collider.gameObject;
            }
            yield return target;
        }

        private IEnumerable DisplayTargeting(GameObject target) {
            if (telegraphInstance == null) {
                    telegraphInstance = Instantiate(telegraphPrefab, Vector3.zero, Quaternion.identity);
            } 
            else {
                telegraphInstance.gameObject.SetActive(true);
            }

            telegraphInstance.localScale = new Vector3(aoeRadius * 2, 1, aoeRadius * 2);

            if(telegraphInstance != null) {
                telegraphInstance.gameObject.SetActive(false);
            }

            yield return null;
        }
    }
}