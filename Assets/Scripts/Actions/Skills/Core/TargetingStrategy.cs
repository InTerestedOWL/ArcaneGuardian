// Based on https://www.udemy.com/course/rpg-shops-abilities/
using UnityEngine;

/*
 * Strategy to determine how to target a skill (e.g. target, aoe, directional, etc.)
 */
namespace AG.Skills.Targeting {
    public abstract class TargetingStrategy : ScriptableObject {
        public abstract void DeclareTargets();
    }
}