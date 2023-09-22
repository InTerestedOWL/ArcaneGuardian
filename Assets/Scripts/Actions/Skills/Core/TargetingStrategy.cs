// Based on https://www.udemy.com/course/rpg-shops-abilities/
using System;
using System.Collections.Generic;
using AG.Skills.Core;
using UnityEngine;

/*
 * Strategy to determine how to target a skill (e.g. target, aoe, directional, etc.)
 */
namespace AG.Skills.Targeting {
    public abstract class TargetingStrategy : ScriptableObject {
        public bool cancelTargeting = false;
        public static Queue<TargetingStrategy> activeStrategies = new Queue<TargetingStrategy>();
        public abstract void DeclareTargets(SkillData data, Action callback);
    }
}