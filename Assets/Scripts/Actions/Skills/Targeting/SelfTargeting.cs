using System;
using System.Collections;
using AG.Skills.Core;
using UnityEngine;

namespace AG.Skills.Targeting
{
    [CreateAssetMenu(fileName = "Self Targeting", menuName = ("Arcane Guardian/Targeting Strategy/Self Targeting"))]
    public class SelfTargeting : TargetingStrategy
    {
        public override void DeclareTargets(SkillData data, Action callback) {
            GameObject user = data.GetUser();
            if (user) {
                data.GetPlayerController().StartCoroutine(StartTargeting(data, callback));
            }
        }

        public IEnumerator StartTargeting(SkillData data, Action finished)
        {
            data.SetTargets(new GameObject[]{data.GetUser()});
            data.SetTargetPosition(data.GetUser().transform.position);
            finished();
            yield return null;
        }
    }
}
