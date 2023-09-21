using AG.Combat;
using AG.Skills.Effects;
using UnityEngine;

namespace AG.Skills {
    // Checks if a target is in range of an AoE skill
    public class AoERangeChecker : MonoBehaviour {
        AoEDoTTracker aoeDoTTracker;

        void Start() {
            // Get Tracker of remaining time for AoE DoT
            aoeDoTTracker = GetComponent<AoEDoTTracker>();
        }

        void OnTriggerEnter(Collider collider) {
            CombatTarget combatTarget = collider.gameObject.GetComponent<CombatTarget>();

            // Check if target is in range of AoE skill
            if (combatTarget != null) {
                DoTAoEValues values = aoeDoTTracker.GetCurrentValues();
                combatTarget.SetInAoERange(true);
                // Filter out targets according to filter strategy
                if (aoeDoTTracker.TargetValid(combatTarget.gameObject)) {
                    // Damage target for remaining time of AoE
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