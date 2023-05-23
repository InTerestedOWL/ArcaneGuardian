using System;
using UnityEngine;

using AG.Actions;
using AG.Skills.Targeting;

namespace AG.Skills {
    [CreateAssetMenu(menuName = ("Arcane Guardian/Skill"))]
    /*
     * Will use properties that follow the strategy pattern
     * Based on https://www.udemy.com/course/rpg-shops-abilities/
     */
    public class Skill : ActionItem {
        [SerializeField]
        TargetingStrategy targetingStrategy = null;
        public override void Use(GameObject user) {
            targetingStrategy.DeclareTargets();
        }
    }
}