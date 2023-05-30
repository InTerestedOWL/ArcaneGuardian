using AG.Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AG.UI {
    public class ToggleMenu : MonoBehaviour {
        public static int menuOpenCounter = 0;

        [SerializeField] 
        InputActionReference toggleMenuActionPlayer = null;

        [SerializeField]
        InputActionReference toggleMenuActionUI = null;
        
        [SerializeField] 
        GameObject menu = null;

        void Start() {
            if (menu) {
                menu.SetActive(false);
            }
            toggleMenuActionPlayer.action.performed += ToggleMenuAction_performed;
            toggleMenuActionUI.action.performed += ToggleMenuAction_performed;
        }

        private void ToggleMenuAction_performed(InputAction.CallbackContext context) {
            menu.SetActive(!menu.activeSelf);
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (menu.activeSelf) {
                ActionMapHandler actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
                ToggleMenu.menuOpenCounter++;
                actionMapHandler.ChangeToActionMap("UI");
            } else {
                CloseMenu(playerObj, true);
            }
        }

        public void CloseMenu(GameObject playerObj = null, bool menuChanged = false) {
            if (playerObj == null) {
                playerObj = GameObject.FindWithTag("Player");
            }
            if (!menuChanged) {
                menu.SetActive(!menu.activeSelf);
            }
            ToggleMenu.menuOpenCounter--;
            if (ToggleMenu.menuOpenCounter <= 0) {
                ActionMapHandler actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
                actionMapHandler.ChangeToActionMap("Player");
            }
        }
    }
}