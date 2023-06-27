using UnityEngine;
using UnityEngine.InputSystem;

namespace AG.Control {
    public class ActionMapHandler : MonoBehaviour {
        private InputActionMap lastPlayerInputActionMap = null;
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


        /// <param name="dynamicallyChangePlayerMap"> 
        /// If dynamicallyChangePlayerMap is true, the last used player input action map will be enabled.
        /// </param>
        public void ChangeToActionMap(string actionMapName, bool dynamicallyChangePlayerMap = true) {
            if (actionMapName == null) {
                return;
            } 
            if (actionMapName.StartsWith("Player")) {
                if (dynamicallyChangePlayerMap) {
                    DisableAllActionMaps();
                    lastPlayerInputActionMap.Enable();
                    return;
                }
            } else {
                SetPreviousUsedPlayerInputActionMap();
            }
            DisableAllActionMaps();
            EnableActionMap(actionMapName);
        }

        private void SetPreviousUsedPlayerInputActionMap() {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            foreach (InputActionMap actionMap in playerInput.actions.actionMaps) {
                if (actionMap.enabled && actionMap.name.StartsWith("Player")) {
                    lastPlayerInputActionMap = actionMap;
                }
            }
        }

        public InputAction GetActionOfCurrentActionMap(string actionName) {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            InputAction action = playerInput.actions.FindAction(actionName);
            return action;
        }
    }
}