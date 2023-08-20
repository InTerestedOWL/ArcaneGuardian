using UnityEngine;
using AG.Skills;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace AG.Combat
{
    public class AoEProjectile : Projectile
    {
        [SerializeField] float aoeRadius = 2;

        float prevDistance = 0;
        float distance = 0;
        bool hasHit = false;

        new void Start() {
            base.Start();

            prevDistance = Vector3.Distance(this.transform.position, GetAimLocation());
        }

        new void Update()
        {
            if (target != null && isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            distance = Vector3.Distance(this.transform.position, GetAimLocation());

            if (distance <= 0.1f || distance > prevDistance)
            {
                if(!hasHit){
                    OnHit();
                }
            }

            prevDistance = distance;
        }

        new void OnTriggerEnter(Collider other)
        {
            return;
        }

        private void OnHit() {
            hasHit = true;

            foreach (GameObject target in GetAoETargets(GetAimLocation()))
            {
                CombatTarget ct = target.GetComponent<CombatTarget>();
                if (ct != null)
                {
                    ct.DamageTarget(damage);
                }
            }

            if (hitEffect != null)
            {
                GameObject hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                Destroy(hitEffectInstance, lifeAfterImpact);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        private IEnumerable<GameObject> GetAoETargets(Vector3 targetPos)
        {
            List<GameObject> targets = new List<GameObject>();
            RaycastHit[] hits = Physics.SphereCastAll(targetPos, aoeRadius, Vector3.up, 0f);
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.tag == "Enemy"){
                    targets.Add(hit.collider.gameObject);
                }
            }
            return targets;
        }

    }

}