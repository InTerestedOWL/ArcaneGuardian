using UnityEngine;
using AG.Actions;
using AG.Skills.Targeting;
using AG.Skills.Filtering;
using AG.Skills.Effects;
using AG.Skills.Core;
using System.Collections;
using AG.Control;
using AG.Audio.Sounds;
using AG.Combat;
using System.Collections.Generic;

namespace AG.Skills {
    /*
     * Will use properties that follow the strategy pattern
     * Based on https://www.udemy.com/course/rpg-shops-abilities/
     * Defines a skill that can be used by the player, enemy or turret.
     */

    [CreateAssetMenu(menuName = ("Arcane Guardian/Skill"))]
    public class Skill : ActionItem {
        [SerializeField] 
        protected int damage = 0;
        // Max Level of skill
        [SerializeField]
        protected int skillCap = 0;
        [SerializeField]
        protected TargetingStrategy targetingStrategy = null;
        [SerializeField]
        protected FilterStrategy filterStrategy = null;
        [SerializeField]
        protected EffectStrategy[] effectStrategies = null;
        // Sounds to play when hit by skill
        [SerializeField]
        protected HitSounds hitSounds = null;
        // Active counter foe AoE skills
        private static int active = 0;
        // Queue of skills to be used when AoE Skills are changed
        private static Queue<SkillQueueEntry> activeSkills = new Queue<SkillQueueEntry>();

        // Defines the use of the skill.
        public override void Use(GameObject user) {
            if (targetingStrategy is not AoETargeting) {
                if (isOnCooldown) {
                    return;
                }
                // Init SkillData with user of skill and damage
                SkillData data = new SkillData(user, damage);
                // Declare targets and process them
                targetingStrategy.DeclareTargets(data, () => ProcessTargets(data));
            } else {
                // Only allow one AoE selection at a time
                if (active == 0) {
                    if (isOnCooldown) {
                        return;
                    }
                    active++;
                    // Init SkillData with user of skill and damage
                    SkillData data = new SkillData(user, damage);
                    // Declare targets and process them
                    targetingStrategy.DeclareTargets(data, () => ProcessTargets(data));
                } else {
                    // Enqueue skill to be used when AoE skills are changed while one is active
                    activeSkills.Enqueue(new SkillQueueEntry { skill = this, user = user });
                }
            }
        }

        // Process targets of strategy and apply effects
        protected void ProcessTargets(SkillData skillData) {
            skillData.SetSkill(this);
            PlayerController playerController = skillData.GetPlayerController();
            if(playerController != null){
                // Start cooldown for skill
                playerController.StartCoroutine(StartCooldown());
            }

            // Filter targets
            skillData.SetFilterStrategy(filterStrategy);
            filterStrategy.FilterTargets(skillData);

            // Apply effects
            foreach(EffectStrategy effectStrategy in effectStrategies) {
                effectStrategy.ApplyEffect(skillData);
            }

            // Show tutorial for Skills
            if(playerController != null) {
                playerController.StartCoroutine(ShowTutorial());
            }
        }

        // Finish AoE skill and use next skill in queue
        // Will only be used when switched while one is active
        public static void FinishSkill() {
            active--;
            if (activeSkills.Count > 0) {
                SkillQueueEntry skillQueueEntry = activeSkills.Dequeue();
                skillQueueEntry.skill.Use(skillQueueEntry.user);
            }
        }
        
        // Start cooldown coroutine.
        public override IEnumerator StartCooldown() {
            currentCooldown = cooldown;
            isOnCooldown = true;
            while(currentCooldown > 0) {
                currentCooldown -= Time.fixedDeltaTime;
                foreach(SlotCooldownUI slotCooldownUI in cooldownUIs) {
                    // Show cooldown animation in UI
                    slotCooldownUI.SetCooldown(currentCooldown, cooldown);
                }
                yield return new WaitForFixedUpdate();
            }
            isOnCooldown = false;
        }

        // Show tutorial for Skills
        public IEnumerator ShowTutorial() {
            yield return new WaitForSeconds(0.5f);
            TutorialHandler.AddTutorialToShow("SkillUsed", "Skillbook");
        }

        public EffectStrategy[] GetEffectStrategies() {
            return effectStrategies;
        }

        public FilterStrategy GetFilterStrategy() {
            return filterStrategy;
        }

        public HitSounds GetHitSounds() {
            return hitSounds;
        }

        public int GetDamage() {
            return damage;
        }

        public int GetSkillCap() {
            return skillCap;
        }
    }

    public class SkillQueueEntry {
        public Skill skill;
        public GameObject user;
    }
}