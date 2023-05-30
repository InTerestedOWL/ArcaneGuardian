// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventories / InventorySlotUI.cs

using UnityEngine;
using GameDevTV.Core.UI.Dragging;
using UnityEngine.InputSystem;
using TMPro;
using AG.Actions;

namespace AG.UI.Draggable {
    public class ActionBarSlotUI : MonoBehaviour, IDragContainer<ActionItem> {
        [SerializeField] 
        InputActionReference hotkeyReference = null;
        string hotkey = "";
        [SerializeField]
        SkillRef skillRef = null;

        private void Awake() {
            SetHotkeyUIDisplay();
        }

        private void Start() {
            hotkeyReference.action.started += TriggerHotkey;
        }

        private void SetHotkeyUIDisplay() {
            if (hotkeyReference != null) {
                // Takes only the first assigned key.
                // TODO: Allow multiple keys to be assigned / Check for other keys.
                hotkey = hotkeyReference.action.GetBindingDisplayString();
                TMP_Text text = GetComponentInChildren<TMP_Text>();
                if (text != null) {
                    text.text = hotkey;
                }
            }
        }

        public int MaxAcceptable(ActionItem item) {
            if (GetItem() == null) {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(ActionItem item, int number) {
            skillRef.SetItem(item);
        }

        public ActionItem GetItem() {
            return skillRef.GetItem();       
        }

        public int GetNumber() {
            return 1;
        }

        public void RemoveItems(int number) {
            skillRef.SetItem(null);
        }

        private void TriggerHotkey(InputAction.CallbackContext context) {
            skillRef.UseCurrentItem();
        }
    }
}

/*
namespace AG.UI.Draggable {
    public class ActionBarSlotUI : MonoBehaviour, IDragContainer<Sprite> {
        [SerializeField] 
        SkillIcon icon = null;
        [SerializeField] 
        InputActionReference hotkeyReference = null;
        string hotkey = "";

        private void Awake() {
            SetHotkeyUIDisplay();
        }

        private void SetHotkeyUIDisplay() {
            if (hotkeyReference != null) {
                // Takes only the first assigned key.
                // TODO: Allow multiple keys to be assigned / Check for other keys.
                hotkey = hotkeyReference.action.GetBindingDisplayString();
                TMP_Text text = GetComponentInChildren<TMP_Text>();
                if (text != null) {
                    text.text = hotkey;
                }
            }
        }

        public int MaxAcceptable(Sprite item) {
            if (GetItem() == null) {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(Sprite item, int number) {
            icon.SetItem(item);
        }

        public Sprite GetItem() {
            return icon.GetItem();
        }

        public int GetNumber() {
            return 1;
        }

        public void RemoveItems(int number) {
            icon.SetItem(null);
        }
    }
}
*/