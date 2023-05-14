using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.AI;

// Based on https://www.udemy.com/course/unityrpg/
public class Movement : MonoBehaviour
{
    bool keysPressed = false;
    Vector3 curMovement = Vector3.zero;

    private void Update() {
        if (keysPressed) {
            GetComponent<NavMeshAgent>().SetDestination(transform.position + curMovement);
        }
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

        GetComponent<NavMeshAgent>().SetDestination(transform.position + curMovement);
    }

    private void OnMoveToPointer() {
        RaycastHit hit;
        Ray curRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        bool hasHit = Physics.Raycast(curRay, out hit);
        if (hasHit)
        {
            GetComponent<NavMeshAgent>().SetDestination(hit.point);
        }
    }
}
