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
        [SerializeField] bool useTargetPoint = true;

        public override void ApplyEffect(SkillData skillData)
        {
            GameObject user = skillData.GetUser();
            // Vector3 spawnPosition = combatTarget.GetHandTransform(isRightHand).position;
            Vector3 spawnPosition = user.transform.position;
            if (useTargetPoint)
            {
                SpawnProjectileForTargetPoint(skillData, spawnPosition);
            }
            else
            {
                SpawnProjectilesForTargets(skillData, spawnPosition);
            }
        }

        private void SpawnProjectileForTargetPoint(SkillData skillData, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPosition;
            projectile.SetTarget(skillData.GetTargetPosition(), skillData.GetUser(), skillData.GetDamage());
        }

        private void SpawnProjectilesForTargets(SkillData skillData, Vector3 spawnPosition)
        {
            foreach (var target in skillData.GetTargets())
            {
                CombatTarget combatTarget = target.GetComponent<CombatTarget>();
                if (combatTarget)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(combatTarget, skillData.GetUser(), skillData.GetDamage());
                }
            }
        }
    }
}
