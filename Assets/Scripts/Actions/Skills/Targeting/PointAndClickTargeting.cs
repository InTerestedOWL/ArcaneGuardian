using System;
using System.Collections;
using System.Collections.Generic;
using AG.Control;
using AG.Skills.Core;
using AG.Combat;
using UnityEngine;
using UnityEngine.InputSystem;


namespace AG.Skills.Targeting
{
    [CreateAssetMenu(fileName = "Point and Click Targeting", menuName = "Arcane Guardian/Targeting Strategy/Point and Click Targeting")]
    public class PointAndClickTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask; 

        public override void DeclareTargets(SkillData data, Action callback)
        {
            GameObject user = data.GetUser();
            if (user)
            {
                data.GetPlayerController().StartCoroutine(StartTargeting(data, callback));
            }
        }

        public IEnumerator StartTargeting(SkillData data, Action callback)
        {
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                CombatTarget ct = raycastHit.collider.gameObject.GetComponent<CombatTarget>();
                if(ct){
                    if(!ct.IsDead()) {
                        data.SetTargetPosition(raycastHit.point);
                        List<GameObject> targets = new List<GameObject>
                        {
                            raycastHit.collider.gameObject
                        };
                        data.SetTargets(targets);

                        callback();
                    }
                }
            }
            yield return null;
        }
    }
}