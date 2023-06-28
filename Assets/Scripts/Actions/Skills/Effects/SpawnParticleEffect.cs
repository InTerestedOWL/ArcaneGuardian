using UnityEngine;
using AG.Skills.Core;
using System.Collections;

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
                skillData.GetPlayerController().StartCoroutine(DisplayParticle(skillData));
            }
        }

        private IEnumerator DisplayParticle(SkillData skillData) {
            Vector3 targetPos = Vector3.zero;
            if (attachToPlayer) {
                targetPos = skillData.GetUser().transform.position;
            } else {
                targetPos = skillData.GetTargetPosition();
            }
            GameObject particle = Instantiate(particlePrefab, targetPos, Quaternion.identity);
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