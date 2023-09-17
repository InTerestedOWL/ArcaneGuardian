using UnityEngine;
using AG.Skills;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;


namespace AG.Combat
{
    public class SpreadAfterImpactProjectile : Projectile
    {
        [SerializeField] float spreadRadius = 5;
        [SerializeField] int maxHit = 10;

        private List<CombatTarget> hitTargets = new List<CombatTarget>();
        private GameObject[] potentialTargets;

        new void OnTriggerEnter(Collider other)
        {
            CombatTarget hitTarget = other.GetComponent<CombatTarget>();
            if (target != null && hitTarget != target) return;
            if (hitTarget == null || hitTarget.IsDead()) return;
            if (other.gameObject == instigator) return;
            hitTarget.DamageTarget(damage, skill);
            hitTargets.Add(hitTarget);

            onHit.Invoke();
            setNextTarget();

            InstantiateOnHitEffect(hitTarget);
        }

        void setNextTarget() {
            if(potentialTargets == null){
                potentialTargets = GameObject.FindGameObjectsWithTag("Enemy").Where(t => t.GetComponent<CombatTarget>()?.currentHealth > 0).ToArray();
            }

            GameObject closestTarget = null;

            float closest = float.MaxValue;
            foreach(GameObject target in potentialTargets) {
                if(!hitTargets.Contains(target.GetComponent<CombatTarget>())) {
                    float distance = Vector3.Distance(target.transform.position, this.transform.position);
                    if(distance < closest && distance <= spreadRadius) {
                        closest = distance;
                        closestTarget = target;
                    }
                }
            }

            if(closestTarget != null && hitTargets.Count() <= maxHit) {
                SetTarget(closestTarget.GetComponent<CombatTarget>(), instigator, damage);
                this.transform.LookAt(GetAimLocation());
            }
            else {
                foreach (GameObject toDestroy in destroyOnHit)
                {
                    Destroy(toDestroy);
                }

                Destroy(gameObject, lifeAfterImpact);
            }
        }
    }
}