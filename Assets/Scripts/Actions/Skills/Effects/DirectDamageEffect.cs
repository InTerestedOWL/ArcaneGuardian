using System.Collections.Generic;
using UnityEngine;
using AG.Combat;
using AG.Skills.Core;

namespace AG.Skills.Effects {
    [CreateAssetMenu(fileName = "Direct Damage Effect", menuName = ("Arcane Guardian/Effect Strategy/Direct Damage Effect"))]
    public class DirectDamageEffect : EffectStrategy {
        [SerializeField]
        int damage = 0;

        public override void ApplyEffect(SkillData skillData) {
            foreach (GameObject target in skillData.GetTargets()) {
                CombatTarget ct = target.GetComponent<CombatTarget>();
                if (ct != null) {
                    ct.DamageTarget(damage);
                }
            }
        }
    }
}