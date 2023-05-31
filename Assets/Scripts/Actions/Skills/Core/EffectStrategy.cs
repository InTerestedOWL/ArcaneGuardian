// Based on https://www.udemy.com/course/rpg-shops-abilities/
using System;
using System.Collections.Generic;
using AG.Skills.Core;
using UnityEngine;

/*
 * Strategy to determine how to apply an effect (e.g. damage, heal, dot, etc.)
 */
namespace AG.Skills.Effects {
    public abstract class EffectStrategy : ScriptableObject {
        public abstract void ApplyEffect(SkillData skillData);
    }
}