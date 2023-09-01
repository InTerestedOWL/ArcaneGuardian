using UnityEngine;
using AG.Skills.Core;
using System.Collections;
using AG.Control;

namespace AG.Skills.Effects {
    [CreateAssetMenu(fileName = "Particle Effect", menuName = ("Arcane Guardian/Effect Strategy/Particle Effect"))]
    public class SpawnParticleEffect : EffectStrategy {
        [SerializeField]
        GameObject particlePrefab = null;
        [SerializeField]
        float duration = 1;
        [SerializeField]
        bool useRadius = true;
        [SerializeField]
        bool attachToPlayer = false;
        [SerializeField]
        bool rotateToTarget = false;

        public override void ApplyEffect(SkillData skillData) {
            if (particlePrefab) {
                PlayerController playerController = skillData.GetPlayerController();
                TurretController turretController = skillData.GetTurretController();
                if(playerController != null) {
                    playerController.StartCoroutine(DisplayParticle(skillData));
                }

                if(turretController != null) {
                    turretController.StartCoroutine(DisplayParticle(skillData));
                }
            }
        }

        private IEnumerator DisplayParticle(SkillData skillData) {
            GameObject particle = null;
            if (attachToPlayer) {
                particle = Instantiate(particlePrefab, skillData.GetUser().transform, false);
            } else {
                particle = Instantiate(particlePrefab, skillData.GetTargetPosition(), Quaternion.identity);
            }
            
            if (rotateToTarget) {
                Vector3 mousePos = skillData.GetTargetPosition();
                mousePos.y = particle.transform.position.y;
                particle.transform.LookAt(mousePos);
            }
            if (useRadius)
                particle.transform.localScale *=  (skillData.GetRadius() * 2);
            if (duration > 0)  {
                yield return new WaitForSeconds(duration);
                Destroy(particle);
            }
        }
    }
}