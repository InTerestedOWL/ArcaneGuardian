using UnityEngine;
using AG.Actions;
using AG.Skills.Targeting;
using AG.Skills.Filtering;
using AG.Skills.Effects;
using AG.Skills.Core;
using System.Collections;
using AG.Control;

namespace AG.Skills {
    /*
     * Will use properties that follow the strategy pattern
     * Based on https://www.udemy.com/course/rpg-shops-abilities/
     */

    // TODO: Combine SkillTree Skill and SkillBook Skill?
    [CreateAssetMenu(menuName = ("Arcane Guardian/Ai Skill"))]
    public class AiSkill : Skill {
        public void Use(GameObject user, GameObject target) {
            if (isOnCooldown) {
                return;
            }
            SkillData data = new SkillData(user, damage);
            data.SetTargets(new GameObject[] { target });

            ProcessTargets(data);
        }
    }
}