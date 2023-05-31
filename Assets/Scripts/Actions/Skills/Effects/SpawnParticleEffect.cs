using UnityEngine;
using AG.Skills.Core;
using System.Collections;

namespace AG.Skills.Effects {
    [CreateAssetMenu(fileName = "Particle Effect", menuName = ("Arcane Guardian/Effect Strategy/Particle Effect"))]
    public class SpawnParticleEffect : EffectStrategy {
        [SerializeField]
        GameObject particlePrefab = null;
        [SerializeField]
        int duration = 1;

        public override void ApplyEffect(SkillData skillData) {
            if (particlePrefab) {
                skillData.GetPlayerController().StartCoroutine(DisplayParticle(skillData));
            }
        }

        private IEnumerator DisplayParticle(SkillData skillData) {
            GameObject particle = Instantiate(particlePrefab, skillData.GetTargetPosition(), Quaternion.identity);
            particle.transform.localScale *=  (skillData.GetRadius() * 2);
            if (duration > 0)  {
                yield return new WaitForSeconds(duration);
                Destroy(particle);
            }
        }
    }
}