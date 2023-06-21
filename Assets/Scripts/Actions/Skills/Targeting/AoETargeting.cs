// Based on https://www.udemy.com/course/rpg-shops-abilities/
using System;
using System.Collections;
using System.Collections.Generic;
using AG.Control;
using AG.Skills.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AG.Skills.Targeting {
    [CreateAssetMenu(fileName = "AoE Targeting", menuName = ("Arcane Guardian/Targeting Strategy/AoE Targeting"))]
    public class AoETargeting : TargetingStrategy {
        [SerializeField]
        LayerMask layerMask;
        [SerializeField] 
        float aoeRadius = 1f;
        [SerializeField]
        Transform telegraphPrefab = null;
        Transform telegraphInstance = null;
        private bool targeting = false;

        public override void DeclareTargets(SkillData data, Action callback) {
            GameObject user = data.GetUser();
            if (user) {
                data.GetPlayerController().StartCoroutine(SelectAoETarget(data, callback));
            }
        }

        private IEnumerator SelectAoETarget(SkillData data, Action callback) {
            ActionMapHandler actionMapHandler = GameObject.FindWithTag("Player").GetComponent<ActionMapHandler>();
            InputAction cancelSpellAction = actionMapHandler.GetActionOfCurrentActionMap("CancelSpell");
            InputAction selectTarget = actionMapHandler.GetActionOfCurrentActionMap("SelectTarget");
            RaycastHit groundHit;
            bool buttonTriggered = false;
            bool hasHit = false;
            if (telegraphInstance == null) {
                telegraphInstance = Instantiate(telegraphPrefab, Vector3.zero, Quaternion.identity);
            } else {
                telegraphInstance.gameObject.SetActive(true);
            }

            telegraphInstance.localScale = new Vector3(aoeRadius * 2, 1, aoeRadius * 2);
            data.SetRadius(aoeRadius);

            targeting = true;
            actionMapHandler.ChangeToActionMap("Casting");

            while (targeting) {
                data.GetPlayerController().CalcPointerHit(out groundHit, out hasHit, layerMask);
                if (hasHit) {
                    telegraphInstance.position = groundHit.point;
                }
                if (buttonTriggered && cancelSpellAction.phase == InputActionPhase.Waiting && selectTarget.phase == InputActionPhase.Waiting) {
                    targeting = false;
                    telegraphInstance.gameObject.SetActive(false);
                    actionMapHandler.ChangeToActionMap("Player");
                    yield break;
                } else if (cancelSpellAction.triggered || selectTarget.triggered) {
                    if (selectTarget.triggered) {
                        data.SetTargets(GetTargets(groundHit.point, hasHit));
                        data.SetTargetPosition(groundHit.point);
                        callback();
                    }
                    buttonTriggered = true;
                }
                yield return null;
            }
            yield return null;
        }

        private IEnumerable<GameObject> GetTargets(Vector3 groundHit, bool hasHit) {
            if (hasHit) {
                RaycastHit[] hits = Physics.SphereCastAll(groundHit, aoeRadius, Vector3.up, 0f);
                foreach (RaycastHit hit in hits) {
                    yield return hit.collider.gameObject;
                }
            }
        }
    }
}