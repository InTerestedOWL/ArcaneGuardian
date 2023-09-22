using System;
using System.Collections;
using System.Collections.Generic;
using AG.Control;
using AG.Skills.Core;
using AG.Combat;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


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
            if (Physics.SphereCast(ray, 0.2f, out raycastHit, 1000, layerMask))
            {
                CombatTarget ct = raycastHit.collider.gameObject.GetComponent<CombatTarget>();
                //TODO: Refactor tag check
                if(ct != null && !ct.IsDead() && ct.gameObject.tag != "Player" && ct.gameObject.tag != "POI" && ct.gameObject.tag != "Turret"){
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
                else {
                    InformationWindow iWindow = GameObject.Find("Information Window").GetComponent<InformationWindow>();
                    iWindow.popupInformationWindow("I need a target!");
                }
            }
            yield return null;
        }
    }
}