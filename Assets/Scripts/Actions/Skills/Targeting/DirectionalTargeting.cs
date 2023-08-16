using System;
using System.Collections;
using System.Collections.Generic;
using AG.Control;
using AG.Skills.Core;
using UnityEngine;
using UnityEngine.InputSystem;


namespace AG.Skills.Targeting
{
    [CreateAssetMenu(fileName = "Directional Targeting", menuName = "Arcane Guardian/Targeting Strategy/Skill Shot Targeting")]
    public class DirectionalTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float groundOffset = 1;

        public override void DeclareTargets(SkillData data, Action callback) {
            GameObject user = data.GetUser();
            if (user) {
                data.GetPlayerController().StartCoroutine(StartTargeting(data, callback));
            }
        }

        public IEnumerator StartTargeting(SkillData data, Action callback)
        {
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                data.SetTargetPosition(raycastHit.point + ray.direction * groundOffset / ray.direction.y);
            }
            callback();

            yield return null;
        }
    }
}