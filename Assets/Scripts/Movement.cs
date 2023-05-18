using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// Based on https://www.udemy.com/course/unityrpg/
public class Movement : MonoBehaviour
{
    bool mouseHold = false;
    bool keysPressed = false;
    IEnumerator mouseMovementCoroutine = null;
    Vector3 curMovement = Vector3.zero;

    private void Update() {
        if (keysPressed) {
            GetComponent<NavMeshAgent>().SetDestination(transform.position + curMovement);
        }
        UpdateAnimator();
    }

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

        DoMovement(transform.position + curMovement);
    }

    private void OnMoveToPointer() {
        mouseHold = !mouseHold;
        if (mouseHold) {
            mouseMovementCoroutine = ContinueMouseMovement();
            StartCoroutine(mouseMovementCoroutine);
        } else {
            if (mouseMovementCoroutine != null) {
                StopCoroutine(mouseMovementCoroutine);
            }
        }
    }

    /*
     * Move to target.
     */
    private void DoMovement(Vector3 newDestination) {
        GetComponent<NavMeshAgent>().SetDestination(newDestination);
    }

    private void UpdateAnimator() {
        // Get velocity for animator
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        // Set Animator blend tree value to movement speed
        GetComponent<Animator>().SetFloat("walkSpeed", speed);
    }

    /*
     * Move to mouse position while mouse button hold down.
     */
    IEnumerator ContinueMouseMovement() {
        while (mouseHold) {
            RaycastHit hit;
            Ray curRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            bool hasHit = Physics.Raycast(curRay, out hit);
            if (hasHit)
            {
                DoMovement(hit.point);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
