using UnityEngine;
using AG.Combat;
using AG.Skills.Core;

namespace AG.Skills.Effects {
    [CreateAssetMenu(fileName = "DoT Damage while in AoE Effect", menuName = ("Arcane Guardian/Effect Strategy/DoT while in AoE Damage Effect"))]
    public class DoTWhileInAoEDamageEffect : EffectStrategy {
        [SerializeField]
        float duration = 0;
        [SerializeField]
        int numberOfTicksInDuration = 0;

        private int damagePerTick = 0;

        public override void ApplyEffect(SkillData skillData) {
            damagePerTick = skillData.GetDamage();

            foreach (GameObject target in skillData.GetTargets()) {
                CombatTarget ct = target.GetComponent<CombatTarget>();
                if (ct != null) {
                    ct.DamageTargetDoTInAoE(damagePerTick, numberOfTicksInDuration, duration, skillData.GetSkill());
                }
            }
        }

        public DoTAoEValues GetValues() {
            return new DoTAoEValues(damagePerTick, numberOfTicksInDuration, duration);
        }
    }

    public class DoTAoEValues {
        public int damagePerTick;
        public int numberOfTicksInDuration;
        public float duration;

        public DoTAoEValues(int damagePerTick, int numberOfTicksInDuration, float duration) {
            this.damagePerTick = damagePerTick;
            this.numberOfTicksInDuration = numberOfTicksInDuration;
            this.duration = duration;
        }
    }
}