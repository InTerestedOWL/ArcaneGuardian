using UnityEngine;
using AG.Skills;
using AG.Skills.Effects;
using System.Collections;
using System.Collections.Generic;

namespace AG.Skills {
    public class AoEDoTTracker : MonoBehaviour {
        [SerializeField]
        Skill skill;
        DoTWhileInAoEDamageEffect effect;
        DoTAoEValues values;

        public void Start() {
            EffectStrategy[] effectStrategies = skill.GetEffectStrategies();

            // Check if effect Strategy contains a DoTWhileInAoEDamageEffect
            foreach (EffectStrategy effectStrategy in effectStrategies) {
                if (effectStrategy.GetType() == typeof(DoTWhileInAoEDamageEffect)) {
                    effect = (DoTWhileInAoEDamageEffect)effectStrategy;
                    break;
                }
            }

            if (effect != null) {
                values = effect.GetValues();

                StartCoroutine(DealDamageOverTimeInAoE(values.damagePerTick, values.numberOfTicksInDuration, values.duration));
            }
        }

        private IEnumerator DealDamageOverTimeInAoE(int damagePerTick, float numberOfTicksInDuration, float duration) {
            for (int i = 0; i < numberOfTicksInDuration; i++) {
                yield return new WaitForSeconds(duration / numberOfTicksInDuration);
                if (values != null) {
                    values.duration -= duration / numberOfTicksInDuration;
                    values.numberOfTicksInDuration -= 1;
                }
            }
        }

        public DoTAoEValues GetCurrentValues() {
            return values;
        }

        // TODO: Filter out targets according to filter strategy (Look for a better way to do this)
        public bool TargetValid(GameObject target) {
            IEnumerable<GameObject> targetEnum = skill.GetFilterStrategy().FilterTargets(GetGameObjectEnumerable(target));
            List<GameObject> targetList = new List<GameObject>(targetEnum);
            if (targetList.Count == 0) return false;
            return true;
        }

        public Skill GetSkill() {
            return skill;
        }

        public IEnumerable<GameObject> GetGameObjectEnumerable(GameObject go) {
            yield return go;
        }
    }
}