using UnityEngine;
using UnityEngine.InputSystem;

namespace AG.Control {
    public class ActionMapHandler : MonoBehaviour {
        public void DisableAllActionMaps() {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            foreach (InputActionMap actionMap in playerInput.actions.actionMaps) {
                actionMap.Disable();
            }
        }

        public void EnableActionMap(string actionMapName) {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            playerInput.actions.FindActionMap(actionMapName).Enable();
        }

        public void ChangeToActionMap(string actionMapName) {
            DisableAllActionMaps();
            EnableActionMap(actionMapName);
        }

        public InputAction GetActionOfCurrentActionMap(string actionName) {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            InputAction action = playerInput.actions.FindAction(actionName);
            return action;
        }
    }
}