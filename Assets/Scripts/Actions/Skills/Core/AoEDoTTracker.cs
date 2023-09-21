using UnityEngine;
using AG.Skills;
using AG.Skills.Effects;
using System.Collections;
using System.Collections.Generic;

namespace AG.Skills {
    // Tracks remaining duration and number of ticks in duration for AoE damage over time
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
                // Get damage and time tick values from effect
                values = effect.GetValues();

                // Start coroutine to track damage over time values
                StartCoroutine(TrackValues(values.damagePerTick, values.numberOfTicksInDuration, values.duration));
            }
        }

        // Track remaining duration and number of ticks in duration
        private IEnumerator TrackValues(int damagePerTick, float numberOfTicksInDuration, float duration) {
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

        // Check if target is valid according to filter strategy
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