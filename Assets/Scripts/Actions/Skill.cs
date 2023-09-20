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
     */

    // TODO: Combine SkillTree Skill and SkillBook Skill?
    [CreateAssetMenu(menuName = ("Arcane Guardian/Skill"))]
    public class Skill : ActionItem {
        [SerializeField] 
        protected int damage = 0;
        [SerializeField]
        protected int skillCap = 0;
        [SerializeField]
        protected TargetingStrategy targetingStrategy = null;
        [SerializeField]
        protected FilterStrategy filterStrategy = null;
        [SerializeField]
        protected EffectStrategy[] effectStrategies = null;
        [SerializeField]
        protected HitSounds hitSounds = null;
        private static int active = 0;
        private static Queue<SkillQueueEntry> activeSkills = new Queue<SkillQueueEntry>();

        public override void Use(GameObject user) {
            if (targetingStrategy is not AoETargeting) {
                if (isOnCooldown) {
                    return;
                }
                SkillData data = new SkillData(user, damage);
                targetingStrategy.DeclareTargets(data, () => ProcessTargets(data));
            } else {
                if (active == 0) {
                    if (isOnCooldown) {
                        return;
                    }
                    active++;
                    SkillData data = new SkillData(user, damage);
                    targetingStrategy.DeclareTargets(data, () => ProcessTargets(data));
                } else {
                    activeSkills.Enqueue(new SkillQueueEntry { skill = this, user = user });
                }
            }
        }

        protected void ProcessTargets(SkillData skillData) {
            skillData.SetSkill(this);
            PlayerController playerController = skillData.GetPlayerController();
            if(playerController != null){
                playerController.StartCoroutine(StartCooldown());
            }

            skillData.SetFilterStrategy(filterStrategy);
            filterStrategy.FilterTargets(skillData);

            foreach(EffectStrategy effectStrategy in effectStrategies) {
                effectStrategy.ApplyEffect(skillData);
            }
            if(playerController != null) {
                playerController.StartCoroutine(ShowTutorial());
            }
        }

        public static void FinishSkill() {
            active--;
            if (activeSkills.Count > 0) {
                SkillQueueEntry skillQueueEntry = activeSkills.Dequeue();
                skillQueueEntry.skill.Use(skillQueueEntry.user);
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