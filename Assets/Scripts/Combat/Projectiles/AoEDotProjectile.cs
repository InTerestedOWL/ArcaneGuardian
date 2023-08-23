using UnityEngine;
using AG.Skills;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;


namespace AG.Combat
{
    public class AoEDotProjectile : Projectile
    {
        [SerializeField] float aoeDotDiameter = 10;
        [SerializeField] float dotDuration = 10;
        [SerializeField] float dotsPerSecond = 1;

        private GameObject hitEffectInstance = null;

        private void OnTriggerEnter(Collider other){
            CombatTarget hitTarget = other.GetComponent<CombatTarget>();
            if (target != null && hitTarget != target) return;
            if (hitTarget == null || hitTarget.IsDead()) return;
            if (other.gameObject == instigator) return;

            speed = 0;

            onHit.Invoke();

            InstantiateOnHitEffect(hitTarget);
            StartCoroutine(DoTTargets());

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
        }

        new void InstantiateOnHitEffect(CombatTarget hitTarget) {
            if (hitEffect != null)
            {
                if(attachHitEffectToTarget){
                    hitEffectInstance = Instantiate(hitEffect, hitTarget.gameObject.transform, false);
                }
                else {
                    // hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                    hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
                }
                hitEffectInstance.transform.localScale = new Vector3(aoeDotDiameter * hitEffect.transform.localScale.x, 1, aoeDotDiameter * hitEffect.transform.localScale.z);
            }
        }

        private IEnumerable<GameObject> GetAoETargets(Vector3 targetPos)
        {
            List<GameObject> targets = new List<GameObject>();
            RaycastHit[] hits = Physics.SphereCastAll(targetPos, aoeDotDiameter/2, Vector3.up, 0f);
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.tag == "Enemy"){
                    targets.Add(hit.collider.gameObject);
                }
            }
            return targets;
        }

        private IEnumerator DoTTargets() {
            float dotTimer = 0;

            while(dotTimer < dotDuration){
                foreach (GameObject target in GetAoETargets(GetAimLocation()))
                {
                    CombatTarget ct = target.GetComponent<CombatTarget>();
                    if (ct != null)
                    {
                        ct.DamageTarget(damage);
                    }
                }
                yield return new WaitForSeconds(1/dotsPerSecond);
                dotTimer += 1/dotsPerSecond;
            }
            Destroy(gameObject, 1);
            Destroy(hitEffectInstance, 1);
            yield return null;
        }

    }

}