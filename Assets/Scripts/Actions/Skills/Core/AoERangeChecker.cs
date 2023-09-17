using AG.Combat;
using AG.Skills.Effects;
using UnityEngine;

namespace AG.Skills {
    public class AoERangeChecker : MonoBehaviour {
        AoEDoTTracker aoeDoTTracker;

        void Start() {
            aoeDoTTracker = GetComponent<AoEDoTTracker>();
        }

        void OnTriggerEnter(Collider collider) {
            CombatTarget combatTarget = collider.gameObject.GetComponent<CombatTarget>();

            if (combatTarget != null) {
                DoTAoEValues values = aoeDoTTracker.GetCurrentValues();
                combatTarget.SetInAoERange(true);
                // Filter out targets according to filter strategy
                if (aoeDoTTracker.TargetValid(combatTarget.gameObject)) {
                    combatTarget.DamageTargetDoTInAoE(values.damagePerTick, values.numberOfTicksInDuration, values.duration, aoeDoTTracker.GetSkill());
                }
            }
        }

        void OnTriggerExit(Collider collider) {
            CombatTarget combatTarget = collider.gameObject.GetComponent<CombatTarget>();

            if (combatTarget != null) {
                combatTarget.SetInAoERange(false);
            }
        }
    }
}