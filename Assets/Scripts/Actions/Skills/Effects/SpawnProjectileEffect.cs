using System;
using System.Linq;
using AG.Combat;
using AG.Skills.Core;
using UnityEngine;

namespace AG.Skills.Effects
{
    [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = ("Arcane Guardian/Effect Strategy/Spawn Projectile Effect"))]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] Projectile projectileToSpawn;
        [SerializeField] int damage;
        // [SerializeField] bool isRightHand = true;
        [SerializeField] bool useTargetPoint = true;

        public override void ApplyEffect(SkillData data)
        {
            GameObject user = data.GetUser();
            // Vector3 spawnPosition = combatTarget.GetHandTransform(isRightHand).position;
            Vector3 spawnPosition = user.transform.position;
            if (useTargetPoint)
            {
                SpawnProjectileForTargetPoint(data, spawnPosition);
            }
            else
            {
                SpawnProjectilesForTargets(data, spawnPosition);
            }
        }

        private void SpawnProjectileForTargetPoint(SkillData data, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPosition;
            projectile.SetTarget(data.GetTargetPosition(), data.GetUser(), damage);
        }

        private void SpawnProjectilesForTargets(SkillData data, Vector3 spawnPosition)
        {
            foreach (var target in data.GetTargets())
            {
                CombatTarget combatTarget = target.GetComponent<CombatTarget>();
                if (combatTarget)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(combatTarget, data.GetUser(), damage);
                }
            }
        }
    }
}
