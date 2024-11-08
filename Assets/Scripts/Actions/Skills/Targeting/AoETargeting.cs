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
        ShapeSelection shapeSelection = ShapeSelection.Circle;
        [SerializeField]
        LayerMask layerMask;
        [SerializeField] 
        float aoeRadius = 1f;
        [SerializeField]
        GameObject telegraphPrefab = null;
        GameObject telegraphInstance = null;
        private bool targeting = false;

        public override void DeclareTargets(SkillData data, Action callback) {
            GameObject user = data.GetUser();
            if (user) {
                activeStrategies.Enqueue(this);
                data.GetPlayerController().StartCoroutine(SelectAoETarget(data, callback));
            }
        }

        private IEnumerator SelectAoETarget(SkillData data, Action callback) {
            cancelTargeting = false;
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

            telegraphInstance.transform.localScale = new Vector3(aoeRadius * 2, 1, aoeRadius * 2);
            data.SetRadius(aoeRadius);

            targeting = true;
            actionMapHandler.ChangeToActionMap("Casting");

            while (targeting) {
                data.GetPlayerController().CalcPointerHit(out groundHit, out hasHit, layerMask);
                if (hasHit) {
                    if (shapeSelection == ShapeSelection.Circle) {
                        telegraphInstance.transform.position = groundHit.point;
                    } else if (shapeSelection == ShapeSelection.Cone) {
                        Vector3 curPos = data.GetUser().transform.position;
                        curPos.y = groundHit.point.y;
                        telegraphInstance.transform.rotation = Quaternion.LookRotation(groundHit.point - curPos);
                        curPos.y += 0.1f;
                        telegraphInstance.transform.position = curPos;
                    }
                }
                if (buttonTriggered && cancelSpellAction.phase == InputActionPhase.Waiting && 
                    (selectTarget.phase == InputActionPhase.Performed || selectTarget.phase == InputActionPhase.Waiting)) {
                    targeting = false;
                    telegraphInstance.gameObject.SetActive(false);
                    if (activeStrategies.Contains(this)) {
                        activeStrategies.Dequeue();
                    }
                    if (activeStrategies.Count == 0) {
                        actionMapHandler.ChangeToActionMap("Player");
                    }
                    Skill.FinishSkill();
                    yield break;
                } else if (cancelSpellAction.triggered || selectTarget.triggered || cancelTargeting) {
                    if (selectTarget.triggered) {
                        data.SetTargets(GetTargets(groundHit.point, hasHit, data.GetUser().transform.position));
                        data.SetTargetPosition(groundHit.point);
                        callback();
                    }
                    buttonTriggered = true;
                }
                yield return null;
            }
            yield return null;
        }

        private IEnumerable<GameObject> GetTargets(Vector3 groundHit, bool hasHit, Vector3 playerPos) {
            if (hasHit) {
                if (shapeSelection == ShapeSelection.Circle) {
                    RaycastHit[] hits = Physics.SphereCastAll(groundHit, aoeRadius, Vector3.up, 0f);
                    foreach (RaycastHit hit in hits) {
                        yield return hit.collider.gameObject;
                    }
                } else if (shapeSelection == ShapeSelection.Cone) {
                    playerPos.y = groundHit.y;
                    RaycastHit[] hits = Physics.SphereCastAll(playerPos, aoeRadius * 2, Vector3.up, 0f);
                    float maxAngle = 45f;
                    foreach (RaycastHit hit in hits) {
                        Vector3 dir = hit.collider.transform.position - playerPos;
                        float angle = Vector3.Angle(dir, telegraphInstance.transform.forward);
                        if (angle < maxAngle || angle > 360 - maxAngle) {
                            yield return hit.collider.gameObject;
                        }
                    }
                }
            }
        }
    }
    
    public enum ShapeSelection {
        Circle,
        Cone
    }
}