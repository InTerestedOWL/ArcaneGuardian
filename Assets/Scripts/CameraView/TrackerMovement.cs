using UnityEngine;

namespace AG.CameraView {
    public class TrackerMovement : MonoBehaviour {
        [SerializeField] Transform targetToFollow;

        // Update is called once per frame
        void LateUpdate() {
            Vector3 playerToCamera = Camera.main.transform.position - targetToFollow.position;
            Ray ray = new Ray(targetToFollow.position, playerToCamera.normalized);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, playerToCamera.magnitude, LayerMask.GetMask("Map"))) {
                transform.position = hit.point + playerToCamera.normalized;
            } else {
                transform.position = targetToFollow.position;
            }
        }
    }
}