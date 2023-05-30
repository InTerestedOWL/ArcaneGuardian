// Based on https://www.udemy.com/course/rpg-shops-abilities/
using System;
using System.Collections;
using System.Collections.Generic;
using AG.Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AG.Skills.Targeting {
    [CreateAssetMenu(fileName = "AoE Targeting", menuName = ("Arcane Guardian/Targeting Strategy/AoE Targeting"))]
    public class AoETargeting : TargetingStrategy {
        [SerializeField]
        LayerMask layerMask;
        [SerializeField] 
        float aoeRadius = 5f;
        [SerializeField]
        Transform telegraphPrefab = null;
        Transform telegraphInstance = null;
        private bool targeting = false;

        public override void DeclareTargets(GameObject user, Action<IEnumerable<GameObject>> callback) {
            PlayerController playerController = user.GetComponent<PlayerController>();
            playerController.StartCoroutine(SelectAoETarget(playerController, callback));
        }

        private IEnumerator SelectAoETarget(PlayerController playerController, Action<IEnumerable<GameObject>> callback) {
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

            targeting = true;
            actionMapHandler.ChangeToActionMap("Casting");

            while (targeting) {
                playerController.CalcPointerHit(out groundHit, out hasHit, layerMask);
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
                        callback(GetTargets(groundHit.point, hasHit));
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