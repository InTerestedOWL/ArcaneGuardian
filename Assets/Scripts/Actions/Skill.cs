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
    [CreateAssetMenu(menuName = ("Arcane Guardian/Skill"))]
    public class Skill : ActionItem {
        [SerializeField]
        protected TargetingStrategy targetingStrategy = null;
        [SerializeField]
        protected FilterStrategy filterStrategy = null;
        [SerializeField]
        protected EffectStrategy[] effectStrategies = null;

        public override void Use(GameObject user) {
            if (isOnCooldown) {
                return;
            }
            SkillData data = new SkillData(user);
            targetingStrategy.DeclareTargets(data, () => ProcessTargets(data));
        }

        protected void ProcessTargets(SkillData skillData) {
            PlayerController playerController = skillData.GetPlayerController();
            if(playerController != null){
                playerController.StartCoroutine(StartCooldown());
            }

            skillData.SetFilterStrategy(filterStrategy);
            filterStrategy.FilterTargets(skillData);
            
            foreach(EffectStrategy effectStrategy in effectStrategies) {
                effectStrategy.ApplyEffect(skillData);
            }
        }
        
        public override IEnumerator StartCooldown() {
            currentCooldown = cooldown;
            isOnCooldown = true;
            while(currentCooldown > 0) {
                currentCooldown -= Time.fixedDeltaTime;
                foreach(SlotCooldownUI slotCooldownUI in cooldownUIs) {
                    slotCooldownUI.SetCooldown(currentCooldown, cooldown);
                }
                yield return new WaitForFixedUpdate();
            }
            isOnCooldown = false;
        }

        public EffectStrategy[] GetEffectStrategies() {
            return effectStrategies;
        }

        public FilterStrategy GetFilterStrategy() {
            return filterStrategy;
        }
    }
}