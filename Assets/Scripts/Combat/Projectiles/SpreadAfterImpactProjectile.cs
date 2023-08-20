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

        private List<CombatTarget> hitTargets = new List<CombatTarget>();

        new void OnTriggerEnter(Collider other)
        {
            CombatTarget hitTarget = other.GetComponent<CombatTarget>();
            if (target != null && hitTarget != target) return;
            if (hitTarget == null || hitTarget.IsDead()) return;
            if (other.gameObject == instigator) return;
            hitTarget.DamageTarget(damage);
            hitTargets.Add(hitTarget);

            onHit.Invoke();
            setNextTarget();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, hitTarget.gameObject.transform.position, transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        void setNextTarget() {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy").Where(t => t.GetComponent<CombatTarget>()?.currentHealth > 0).ToArray();
            GameObject closestTarget = null;

            float closest = float.MaxValue;
            foreach(GameObject target in targets) {
                if(!hitTargets.Contains(target.GetComponent<CombatTarget>())) {
                    float distance = Vector3.Distance(target.transform.position, this.transform.position);
                    if(distance < closest && distance <= spreadRadius) {
                        closest = distance;
                        closestTarget = target;
                    }
                }
            }

            if(closestTarget != null) {
                SetTarget(closestTarget.GetComponent<CombatTarget>(), instigator, damage);
                this.transform.LookAt(GetAimLocation());
            }
            else {
                speed = 0;
            }
        }
    }
}