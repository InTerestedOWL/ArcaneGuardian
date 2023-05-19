using UnityEngine;

namespace AG.CameraView {
    public class TrackerMovement : MonoBehaviour {
        [SerializeField] Transform targetToFollow;

        // Update is called once per frame
        void LateUpdate() {
            transform.position = targetToFollow.position;
        }
    }
}