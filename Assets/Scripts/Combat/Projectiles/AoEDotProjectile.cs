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
        private bool hasHit = false;

        private new void OnTriggerEnter(Collider other){
            //Ignore Player and POI
            if(instigator.tag == "EnemyWeapon") {
                if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWeapon")
                    return;
            }
            else {
                if(other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerWeapon" || other.gameObject.tag == "POI" || other.gameObject.tag == "Turret")
                    return;
            }

            if(hasHit)
                return;
            hasHit = true;

            speed = 0;

            onHit.Invoke();

            if (hitEffect != null)
            {
                hitEffectInstance = Instantiate(hitEffect, this.transform.position, Quaternion.identity);
                hitEffectInstance.transform.localScale = new Vector3(aoeDotDiameter * hitEffect.transform.localScale.x, 1, aoeDotDiameter * hitEffect.transform.localScale.z);
            }

            StartCoroutine(DoTTargets());

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }
        }

        private IEnumerator DoTTargets() {
            float dotTimer = 0;

            while(dotTimer < dotDuration){
                foreach (GameObject target in GetAoETargets(this.transform.position, aoeDotDiameter))
                {
                    CombatTarget ct = target.GetComponent<CombatTarget>();
                    if (ct != null)
                    {
                        ct.DamageTarget(damage, skill);
                    }
                }
                yield return new WaitForSeconds(1/dotsPerSecond);
                dotTimer += 1/dotsPerSecond;
            }
            Destroy(hitEffectInstance);
            Destroy(gameObject, 0.01f);
            yield return null;
        }
    }
}