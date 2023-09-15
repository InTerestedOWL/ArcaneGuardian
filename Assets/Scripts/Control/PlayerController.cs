// Based on https://www.udemy.com/course/unityrpg/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using AG.MovementCore;
using AG.Combat;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace AG.Control {
    public class PlayerController : MonoBehaviour {
        LayerMask ignoreRaycastLayer;
        int uiWindowLayerID;
        bool mouseHold = false;
        bool keysPressed = false;
        bool isLookAtMousePosEnabled = true;
        //IEnumerator mouseMovementCoroutine = null;
        Vector3 curMovement = Vector3.zero;

        void Start() {
            ignoreRaycastLayer = LayerMask.GetMask("Ignore Raycast");
            uiWindowLayerID = LayerMask.NameToLayer("UI Window");
        }

        // Update is called once per frame
        void Update() {
            HandleMovement();
            if(isLookAtMousePosEnabled){
                LookAtMousePos();
            }
        }

        // Movement for keyboard input
        private void HandleMovement() {
            if (keysPressed) {
                Vector3 pos = transform.position;
                pos.y += 10;
                RaycastHit hit;
                bool hasHit = Physics.Raycast(pos, transform.position + curMovement - pos, out hit, Mathf.Infinity, LayerMask.GetMask("Map"));
                if (hasHit && hit.point.y > 0.1f) {
                    GetComponent<Movement>().DoMovement(hit.point);
                }
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
            StartCoroutine(ShowTutorial());
        }

        private IEnumerator ShowTutorial() {
            yield return new WaitForSeconds(1f);
            TutorialHandler.AddTutorialToShow("Crystal", "Movement");
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
            Ray ray;
            CalcPointerHit(out hit, out ray, out hasHit, layerMask);
        }

        public void CalcPointerHit(out RaycastHit hit, out Ray ray, out bool hasHit, LayerMask layerMask = default){
            if (layerMask == default) {
                layerMask = ~ignoreRaycastLayer;
            }

            Vector2 mousePos = Mouse.current.position.ReadValue();
            // Only move when not over UI
            // TODO: Check if user is not dragging something
            if (!checkIfOverUI(mousePos)) {
                ray = Camera.main.ScreenPointToRay(mousePos);

                hasHit = Physics.Raycast(ray, out hit, 1000, layerMask);
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f);
            } else {
                hasHit = false;
                hit = new RaycastHit();
                ray = new Ray();
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

        public void EnableLookAtMousePos() {
            isLookAtMousePosEnabled = true;
        }

        public void DisableLookAtMousePos() {
            isLookAtMousePosEnabled = false;
        }
    }
}
