using UnityEngine;
using System.Collections.Generic;
using AG.Actions;
using AG.Skills.Targeting;
using AG.Skills.Filtering;

namespace AG.Skills {
    /*
     * Will use properties that follow the strategy pattern
     * Based on https://www.udemy.com/course/rpg-shops-abilities/
     */

    [CreateAssetMenu(menuName = ("Arcane Guardian/Skill"))]
    public class Skill : ActionItem {
        [SerializeField]
        TargetingStrategy targetingStrategy = null;
        [SerializeField]
        FilterStrategy filterStrategy = null;
        public override void Use(GameObject user) {
            targetingStrategy.DeclareTargets(user, ProcessTargets);
        }

        private void ProcessTargets(IEnumerable<GameObject> targets) {
            targets = filterStrategy.FilterTargets(targets);

            foreach (GameObject target in targets) {
                Debug.Log("Target: " + target.name);
            }
        }
    }
}