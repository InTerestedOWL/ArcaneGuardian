using UnityEngine;
using AG.Combat;
using AG.Skills.Core;
using System.Collections;
using System.Collections.Generic;
using AG.Skills.Filtering;

namespace AG.Skills.Effects {
    [CreateAssetMenu(fileName = "AoE Effect Over Time", menuName = ("Arcane Guardian/Effect Strategy/AoE Effect Over Time"))]
    public class AoEOverTimeEffect : EffectStrategy {
        //Wenn die Zahl negativ ist, wird das Target geheilt, wenn positiv, wird es geschädigt
        [SerializeField]
        int damagePerTick = 0;
        [SerializeField]
        float duration = 0;
        [SerializeField]
        int ticksPerSecond = 0;


        private bool isDamageOverTime = true;

        public override void ApplyEffect(SkillData skillData) {
            if(damagePerTick > 0) {
                isDamageOverTime = true;
            } else {
                isDamageOverTime = false;
            }

            skillData.GetPlayerController().StartCoroutine(ApplyOverTimeEffect(skillData));
        }

        private IEnumerator ApplyOverTimeEffect(SkillData skillData) {
            float dotTimer = 0;

            while(dotTimer < duration){
                foreach (GameObject target in GetAoETargets(skillData))
                {
                    CombatTarget ct = target.GetComponent<CombatTarget>();
                    if (ct != null)
                    {
                        if(isDamageOverTime) {
                            ct.DamageTarget(damagePerTick, skillData.GetSkill());
                        } else {
                            ct.HealTarget(-damagePerTick, skillData.GetSkill());
                        }
                    }
                }
                yield return new WaitForSeconds(1/ticksPerSecond);
                dotTimer += 1/ticksPerSecond;
                GetAoETargets(skillData);
            }
            yield return null;
        }

        private IEnumerable<GameObject> GetAoETargets(SkillData skillData)
        {
            List<GameObject> targets = new List<GameObject>();
            RaycastHit[] hits = Physics.SphereCastAll(skillData.GetTargetPosition(), skillData.GetRadius(), Vector3.up, 0f);
            foreach (RaycastHit hit in hits)
            {
                targets.Add(hit.collider.gameObject);
            }
            FilterStrategy filterStrategy = skillData.GetFilterStrategy();
            return filterStrategy.FilterTargets(targets);
        }
    }
}