﻿using UnityEngine;
using AG.Skills;
using UnityEngine.Events;
using System.Collections.Generic;

namespace AG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float speed = 1;
        [SerializeField] protected bool isHoming = true;
        [SerializeField] protected GameObject hitEffect = null;
        [SerializeField] protected bool attachHitEffectToTarget = false;
        [SerializeField] protected float maxLifeTime = 10;
        [SerializeField] protected GameObject[] destroyOnHit = null;
        [SerializeField] protected float lifeAfterImpact = 2;
        [SerializeField] protected UnityEvent onHit;
        [SerializeField] protected Skill skill = null;

        protected CombatTarget target = null;
        protected Vector3 targetPoint;
        protected GameObject instigator = null;
        protected int damage = 0;

        protected void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        protected void Update()
        {
            if (target != null && isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(CombatTarget target, GameObject instigator, int damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, int damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        public void SetTarget(GameObject instigator, int damage, CombatTarget target=null, Vector3 targetPoint=default)
        {
            this.target = target;
            this.targetPoint = targetPoint;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        protected Vector3 GetAimLocation()
        {
            if (target == null)
            {
                return targetPoint;
            }
            Collider targetCollider = target.GetComponent<Collider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            
            return targetCollider.bounds.center;
        }


        //TODO: Refactor Enemy/Frineldy instigator check
        protected void OnTriggerEnter(Collider other)
        {
            CombatTarget hitTarget = other.GetComponent<CombatTarget>();
            // if (target != null && hitTarget != target) return;
            if (hitTarget == null || hitTarget.IsDead()) return;
            // if (other.gameObject == instigator) return;

            if(instigator.tag == "EnemyWeapon") {
                if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyWeapon")
                    return;
            }
            else {
                if(other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerWeapon" || other.gameObject.tag == "POI" || other.gameObject.tag == "Turret")
                    return;
            }

            hitTarget.DamageTarget(damage, skill);

            speed = 0;

            onHit.Invoke();

           InstantiateOnHitEffect(hitTarget);

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);

        }

        protected void InstantiateOnHitEffect(CombatTarget hitTarget){
            if (hitEffect != null)
            {
                GameObject hitEffectInstance = null;

                if(attachHitEffectToTarget){
                    hitEffectInstance = Instantiate(hitEffect, hitTarget.gameObject.transform, false);
                }
                else {
                    // hitEffectInstance = Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
                    hitEffectInstance = Instantiate(hitEffect, this.transform.position, transform.rotation);
                }

                Destroy(hitEffectInstance, lifeAfterImpact);
            }
        }

        protected IEnumerable<GameObject> GetAoETargets(Vector3 targetPos, float aoeDiameter)
        {
            List<GameObject> targets = new List<GameObject>();
            RaycastHit[] hits = Physics.SphereCastAll(targetPos, aoeDiameter/2, Vector3.up, 0f);
            foreach (RaycastHit hit in hits)
            {
                if(instigator.tag == "EnemyWeapon") {
                    if(hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "POI" || hit.collider.gameObject.tag == "Turret")
                        targets.Add(hit.collider.gameObject);
                }
                else {
                    if(hit.collider.gameObject.tag == "Enemy"){
                        targets.Add(hit.collider.gameObject);
                }
                }
            }
            return targets;
        }
    }
}