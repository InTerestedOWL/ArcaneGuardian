// Based on https://www.udemy.com/course/unityrpg/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using AG.MovementCore;
using AG.Combat;

namespace AG.Control {
    public class PlayerController : MonoBehaviour {
        bool mouseHold = false;
        bool keysPressed = false;
        IEnumerator mouseMovementCoroutine = null;
        Vector3 curMovement = Vector3.zero;

        // Update is called once per frame
        void Update() {
            HandleMovement();
            // TODO: BoxCollider on Sword Animation
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

        private void OnMoveToPointer() {
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
        }

        private void OnBasicAttack() {
            RaycastHit hit;
            bool hasHit;
            CalcPointerHit(out hit, out hasHit);
            if (hasHit) {
                transform.LookAt(hit.point, Vector3.up);
                GetComponent<BasicCombat>().Attack();
            }
        }

        /*
        * Move to mouse position while mouse button hold down.
        */
        IEnumerator ContinuousMouseMovement() {
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

        private static void CalcPointerHit(out RaycastHit hit, out bool hasHit) {
            Ray curRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            hasHit = Physics.Raycast(curRay, out hit, 1000, ~LayerMask.GetMask("Ignore Raycast"));
            Debug.DrawRay(curRay.origin, curRay.direction * 1000, Color.red, 1f);
        }
    }
}
