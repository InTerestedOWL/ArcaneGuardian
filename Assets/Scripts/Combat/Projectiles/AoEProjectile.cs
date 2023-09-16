using UnityEngine;
using AG.Skills;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace AG.Combat
{
    public class AoEProjectile : Projectile
    {
        [SerializeField] float aoeDiameter = 2;
        [SerializeField] Skill skill;

        // float prevDistance = 0;
        // float distance = 0;
        bool hasHit = false;

        // new void Start() {
        //     base.Start();

        //     prevDistance = Vector3.Distance(this.transform.position, GetAimLocation());
        // }

        // new void Update()
        // {
        //     if (target != null && isHoming && !target.IsDead())
        //     {
        //         transform.LookAt(GetAimLocation());
        //     }
            
        //     transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //     distance = Vector3.Distance(this.transform.position, GetAimLocation());

        //     if (distance <= 0.1f || distance > prevDistance)
        //     {
        //         if(!hasHit){
        //             OnHit();
        //         }
        //     }

        //     prevDistance = distance;
        // }

        new void OnTriggerEnter(Collider other)
        {   
            //Ignore Player and POI if instigator is Friendly unit
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

            foreach (GameObject target in GetAoETargets(this.transform.position, aoeDiameter))
            {
                CombatTarget ct = target.GetComponent<CombatTarget>();
                if (ct != null)
                {
                    ct.DamageTarget(damage, skill);
                }
            }

            if (hitEffect != null)
            {
                GameObject hitEffectInstance = Instantiate(hitEffect, this.transform.position, transform.rotation);
                Destroy(hitEffectInstance, lifeAfterImpact);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        // private void OnHit() {
        //     hasHit = true;
        //     speed = 0;

        //     foreach (GameObject target in GetAoETargets(GetAimLocation()))
        //     {
        //         CombatTarget ct = target.GetComponent<CombatTarget>();
        //         if (ct != null)
        //         {
        //             ct.DamageTarget(damage);
        //         }
        //     }

        //     if (hitEffect != null)
        //     {
        //         GameObject hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        //         Destroy(hitEffectInstance, lifeAfterImpact);
        //     }

        //     foreach (GameObject toDestroy in destroyOnHit)
        //     {
        //         Destroy(toDestroy);
        //     }

        //     Destroy(gameObject, lifeAfterImpact);
        // }
    }

}