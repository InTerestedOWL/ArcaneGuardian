﻿using UnityEngine;
using AG.Skills;
using UnityEngine.Events;

namespace AG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float speed = 1;
        [SerializeField] protected bool isHoming = true;
        [SerializeField] protected GameObject hitEffect = null;
        [SerializeField] protected float maxLifeTime = 10;
        [SerializeField] protected GameObject[] destroyOnHit = null;
        [SerializeField] protected float lifeAfterImpact = 2;
        [SerializeField] protected UnityEvent onHit;

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
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        protected void OnTriggerEnter(Collider other)
        {
            CombatTarget hitTarget = other.GetComponent<CombatTarget>();
            if (target != null && hitTarget != target) return;
            if (hitTarget == null || hitTarget.IsDead()) return;
            if (other.gameObject == instigator) return;
            hitTarget.DamageTarget(damage);

            speed = 0;

            onHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);

        }

    }

}