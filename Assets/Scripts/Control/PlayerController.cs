// Based on https://www.udemy.com/course/unityrpg/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using AG.MovementCore;
using AG.Combat;
using System.Collections.Generic;

namespace AG.Control {
    public class PlayerController : MonoBehaviour {
        LayerMask ignoreRaycastLayer;
        int uiWindowLayerID;
        bool mouseHold = false;
        bool keysPressed = false;
        IEnumerator mouseMovementCoroutine = null;
        Vector3 curMovement = Vector3.zero;

        void Start() {
            ignoreRaycastLayer = LayerMask.GetMask("Ignore Raycast");
            uiWindowLayerID = LayerMask.NameToLayer("UI Window");
        }

        // Update is called once per frame
        void Update() {
            HandleMovement();
            // TODO Animation
            LookAtMousePos();
        }

        // Movement for keyboard input
        private void HandleMovement() {
            if (keysPressed) {
                GetComponent<Movement>().DoMovement(transform.position + curMovement);
            }
        }

        // Handle movement for keyboard input
        private void OnMoveToDirection(InputValue value) {
            Vector2 curDirection = value.Get<Vector2>();
            if (curDirection != Vector2.zero) {
                keysPressed = true;
            } else {
                keysPressed = false;
            }

            // TODO: Ggf. anpassen an "Stopping Distance" des NavMeshAgent
            curMovement = new Vector3(curDirection.x, 0, curDirection.y);
            // Moving in the direction of the camera
            curMovement = Camera.main.transform.TransformDirection(curMovement);
            curMovement.y = 0;

            GetComponent<Movement>().DoMovement(transform.position + curMovement);
        }

        // Handle movement for mouse input - Deactivated
        /*private void OnMoveToPointer() {
            mouseHold = !mouseHold;
            if (mouseHold) {
                mouseMovementCoroutine = ContinuousMouseMovement();
                StartCoroutine(mouseMovementCoroutine);
            }
            else {
                if (mouseMovementCoroutine != null) {
                    StopCoroutine(mouseMovementCoroutine);
                }
            }
        }*/

        private void OnBasicAttack() {
            RaycastHit hit;
            bool hasHit;
            CalcPointerHit(out hit, out hasHit);
            if (hasHit) {
                GetComponent<BasicCombat>().Attack();
            }
        }

        private void LookAtMousePos() {
            RaycastHit hit;
            bool hasHit;
            CalcPointerHit(out hit, out hasHit);
            if (hasHit) {
                Vector3 lookAtPos = hit.point;
                lookAtPos.y = transform.position.y;
                transform.LookAt(lookAtPos, Vector3.up);
            }
        }

        /*
        * Move to mouse position while mouse button hold down.
        */
        IEnumerator ContinuousMouseMovement() {
            // TODO Stop if menu is opened - currently bugged when menu is opened while moving
            while (mouseHold) {
                RaycastHit hit;
                bool hasHit;
                CalcPointerHit(out hit, out hasHit);
                if (hasHit) {
                    GetComponent<Movement>().DoMovement(hit.point);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public void CalcPointerHit(out RaycastHit hit, out bool hasHit, LayerMask layerMask = default) {
            if (layerMask == default) {
                layerMask = ~ignoreRaycastLayer;
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();
            // Only move when not over UI
            // TODO: Check if user is not dragging something
            if (!checkIfOverUI(mousePos)) {
                Ray curRay = Camera.main.ScreenPointToRay(mousePos);

                hasHit = Physics.Raycast(curRay, out hit, 1000, layerMask);
                Debug.DrawRay(curRay.origin, curRay.direction * 1000, Color.red, 1f);
            } else {
                hasHit = false;
                hit = new RaycastHit();
            }
        }

        // Check if mouse is over UI
        private bool checkIfOverUI(Vector2 mousePos) {
            bool overUI = false;

            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = mousePos;

            List<RaycastResult> raycastResultsList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
            for (int i = 0; i < raycastResultsList.Count; i++) {
                if (raycastResultsList[i].gameObject.layer == uiWindowLayerID) {
                    overUI = true;
                    break;
                }
            }
            return overUI;
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
