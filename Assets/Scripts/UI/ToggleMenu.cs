using System.Collections.Generic;
using System.Linq;
using AG.Control;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AG.UI {
    public class ToggleMenu : MonoBehaviour {
        private List<System.Action<InputAction.CallbackContext>> toggleMenuActions = new List<System.Action<InputAction.CallbackContext>>();
        public static int menuOpenCounter = 0;

        [SerializeField]
        List<MenuEntry> menuEntries;

        [SerializeField]
        GameObject settingsMenu;

        [SerializeField]
        GameObject pauseMenu;

        [SerializeField]
        public bool preventInput = false;
        private GlobalAudioSystem globalAudioSystem;

        void Start() {
            globalAudioSystem = GameObject.Find("Main Camera").GetComponent<GlobalAudioSystem>();
            foreach (MenuEntry menuEntry in menuEntries) {
                if (menuEntry.menu) {
                    menuEntry.menu.SetActive(false);
                }
                foreach (InputActionReference toggleMenuActionPlayer in menuEntry.toggleMenuActionsPlayer) {
                    System.Action<InputAction.CallbackContext> action = ctx => ToggleMenuAction_performed(ctx, menuEntry);
                    toggleMenuActionPlayer.action.performed += action;
                    toggleMenuActions.Add(action);
                }
                System.Action<InputAction.CallbackContext> uiAction = ctx => ToggleMenuAction_performed(ctx, menuEntry);
                menuEntry.toggleMenuActionUI.action.performed += uiAction;
                toggleMenuActions.Add(uiAction);
            }
        }

        private void ToggleMenuAction_performed(InputAction.CallbackContext context, MenuEntry curMenuEntry) {
            if (!TutorialHandler.tutorialActive && !preventInput) {
                curMenuEntry.menu.SetActive(!curMenuEntry.menu.activeSelf);
                GameObject playerObj = GameObject.Find("Player");
                if (curMenuEntry.menu.activeSelf) {
                    if (globalAudioSystem) 
                        globalAudioSystem.PlayUIPopupOpenSound();
                    ActionMapHandler actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
                    ToggleMenu.menuOpenCounter++;
                    actionMapHandler.ChangeToActionMap("UI");
                } else {
                    CloseMenu(curMenuEntry, playerObj, true);
                }
            }
        }

        public void CloseMenu(GameObject curMenuObj, GameObject playerObj = null, bool menuChanged = false) {
            MenuEntry menuEntry = menuEntries.Find(menuEntry => menuEntry.menu == curMenuObj);
            if (menuEntry != null) {
                CloseMenu(menuEntry, playerObj, menuChanged);
            }
        }

        public void CloseMenu(MenuEntry curMenu, GameObject playerObj = null, bool menuChanged = false) {
            if (globalAudioSystem)
                globalAudioSystem.PlayUIPopupCloseSound();
            if (curMenu.menu == pauseMenu && settingsMenu.activeSelf) {
                CloseSettingsMenu();
            }
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
                // Look for GameObjects with name InfoBox and close them, if they are active
                GameObject[] activeInfoBoxes = GameObject.FindObjectsOfType<GameObject>()
                    .Where(go => go.activeInHierarchy && go.name == "InfoBox")
                    .ToArray();

                foreach (GameObject infoBox in activeInfoBoxes) {
                    infoBox.SetActive(false);
                }
            }
        }

        public void ToggleWithoutInput(GameObject menu) {
            menu.SetActive(!menu.activeSelf);
            GameObject playerObj = GameObject.Find("Player");
            if (menu.activeSelf) {
                ActionMapHandler actionMapHandler = playerObj.GetComponent<ActionMapHandler>();
                ToggleMenu.menuOpenCounter++;
                actionMapHandler.ChangeToActionMap("UI");
            } else {
                MenuEntry menuEntry = new MenuEntry {
                    menu = menu
                };
                CloseMenu(menuEntry, playerObj, true);
            }
        }

        void OnDestroy() {
            foreach (var action in toggleMenuActions) {
                foreach (MenuEntry menuEntry in menuEntries) {
                    foreach (InputActionReference toggleMenuActionPlayer in menuEntry.toggleMenuActionsPlayer) {
                        toggleMenuActionPlayer.action.performed -= action;
                    }
                    menuEntry.toggleMenuActionUI.action.performed -= action;
                }
            }
            menuOpenCounter = 0;
        }

        public void OpenSettingsMenu() {
            settingsMenu.GetComponent<SettingsHandler>().OpenSettings();
        }

        public void CloseSettingsMenu() {
            settingsMenu.GetComponent<SettingsHandler>().CloseSettings();
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