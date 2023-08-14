using System.Collections.Generic;
using UnityEngine;
using AG.Combat;
using AG.Skills.Core;
using AG.Control;
using System.Collections;
using System.Linq;
using System;

namespace AG.Skills.Effects
{
    [CreateAssetMenu(fileName = "Projectile Damage Effect", menuName = ("Arcane Guardian/Effect Strategy/Projectile Damage Effect"))]
    public class ProjectileDamageEffect : EffectStrategy
    {
        [SerializeField]
        int damage = 0;
        [SerializeField]
        float aoeRadius = 2;
        [SerializeField]
        bool displayTargeting = false;
        [SerializeField]
        float velocity = 10;
        [SerializeField]
        GameObject projectilePrefab = null;
        [SerializeField]
        //Kann ungesetzt bleiben
        GameObject explosionPrefab = null;
        [SerializeField]
        GameObject targetingPrefab = null;

        private GameObject projectileInstance = null;
        private GameObject targetingInstance = null;
        private GameObject explosionInstance = null;

        public override void ApplyEffect(SkillData skillData)
        {
            if (projectilePrefab)
            {
                PlayerController playerController = skillData.GetPlayerController();
                TurretController turretController = skillData.GetTurretController();
                if (playerController != null)
                {
                    playerController.StartCoroutine(LaunchProjectile(skillData));
                }

                if (turretController != null)
                {
                    turretController.StartCoroutine(LaunchProjectile(skillData));
                }
            }
        }

        private IEnumerator LaunchProjectile(SkillData skillData)
        {
            Vector3 targetPos = skillData.GetTargetPosition();
            Vector3 userPos = skillData.GetUser().transform.position;
            projectileInstance = Instantiate(projectilePrefab, userPos, Quaternion.identity);
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
            Vector3 direction = (targetPos - userPos).normalized;
            rb.velocity = direction * velocity;

            if (displayTargeting == true)
            {
                DisplayTargeting(targetPos);
            }

            float prevDistance = Vector3.Distance(projectileInstance.transform.position, targetPos);

            while (true)
            {
                float distance = Vector3.Distance(projectileInstance.transform.position, targetPos);

                if (distance <= 0.1f || distance > prevDistance)
                {
                    break;
                }

                prevDistance = distance;

                yield return null;
            }

            Destroy(targetingInstance);
            Destroy(projectileInstance);

            foreach (GameObject target in GetAoETargets(targetPos))
            {
                CombatTarget ct = target.GetComponent<CombatTarget>();
                if (ct != null)
                {
                    ct.DamageTarget(damage);
                }
            }

            if (explosionPrefab != null)
            {
                DisplayExlposion(targetPos);
            }

            yield return new WaitForSeconds(1);

            //Destroy if not null
            if(projectileInstance != null) {
                Destroy(projectileInstance);
            }
            if(targetingInstance != null) {
                Destroy(targetingInstance);
            }
            if(explosionInstance != null) {
                Destroy(explosionInstance);
            }

            yield return null;
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

        private void DisplayTargeting(Vector3 targetPos)
        {
            if (targetingInstance == null)
            {
                targetingInstance = Instantiate(targetingPrefab, targetPos, Quaternion.identity);
            }
            else
            {
                targetingInstance.transform.position = targetPos;
                targetingInstance.gameObject.SetActive(true);
            }

            targetingInstance.transform.localScale = new Vector3(aoeRadius, 1, aoeRadius);
        }

        //Spawns Explosion as a Child of the Projectile
        // private void DisplayExlposion()
        // {
        //     explosionInstance = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity);
        //     explosionInstance.transform.SetParent(projectileInstance.transform, false);
        //     explosionInstance.transform.localScale = new Vector3(aoeRadius * 2, aoeRadius * 2, aoeRadius * 2);

        //     Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
        //     rb.velocity = Vector3.zero;
        // }

        //Spawns Explosion at the Target Position
        private void DisplayExlposion(Vector3 targetPos)
        {
            explosionInstance = Instantiate(explosionPrefab, targetPos, Quaternion.identity);
            // Funktioniert bei particle systems nicht :(
            // explosionInstance.transform.localScale = new Vector3(aoeRadius, aoeRadius, aoeRadius);
            foreach(Transform child in explosionInstance.transform) {
                child.localScale = new Vector3(aoeRadius, aoeRadius, aoeRadius);
            }
        }
    }
}