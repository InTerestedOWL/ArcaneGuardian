using System.Collections.Generic;
using AG.Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AG.UI {
    public class ToggleMenu : MonoBehaviour {
        public static int menuOpenCounter = 0;

        [SerializeField]
        List<MenuEntry> menuEntries;

        void Start() {
            foreach (MenuEntry menuEntry in menuEntries) {
                if (menuEntry.menu) {
                    menuEntry.menu.SetActive(false);
                }
                foreach (InputActionReference toggleMenuActionPlayer in menuEntry.toggleMenuActionsPlayer) {
                    toggleMenuActionPlayer.action.performed += ctx => ToggleMenuAction_performed(ctx, menuEntry);
                }
                menuEntry.toggleMenuActionUI.action.performed += ctx => ToggleMenuAction_performed(ctx, menuEntry);
            }
        }

        private void ToggleMenuAction_performed(InputAction.CallbackContext context, MenuEntry curMenuEntry) {
            curMenuEntry.menu.SetActive(!curMenuEntry.menu.activeSelf);
            GameObject playerObj = GameObject.Find("Player");
            if (curMenuEntry.menu.activeSelf) {
                ActionMapHandler actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
                ToggleMenu.menuOpenCounter++;
                actionMapHandler.ChangeToActionMap("UI");
            } else {
                CloseMenu(curMenuEntry, playerObj, true);
            }
        }

        public void CloseMenu(GameObject curMenuObj, GameObject playerObj = null, bool menuChanged = false) {
            MenuEntry menuEntry = menuEntries.Find(menuEntry => menuEntry.menu == curMenuObj);
            if (menuEntry != null) {
                CloseMenu(menuEntry, playerObj, menuChanged);
            }
        }

        public void CloseMenu(MenuEntry curMenu, GameObject playerObj = null, bool menuChanged = false) {
            if (playerObj == null) {
                playerObj = GameObject.Find("Player");
            }
            if (!menuChanged) {
                curMenu.menu.SetActive(!curMenu.menu.activeSelf);
            }
            ToggleMenu.menuOpenCounter--;
            if (ToggleMenu.menuOpenCounter <= 0) {
                ActionMapHandler actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
                actionMapHandler.ChangeToActionMap("Player");
            }
        }
    }
}

[System.Serializable]
public class MenuEntry {
    [SerializeField]
    public List<InputActionReference> toggleMenuActionsPlayer = null;

    [SerializeField]
    public InputActionReference toggleMenuActionUI = null;

    [SerializeField]
    public GameObject menu = null;
}