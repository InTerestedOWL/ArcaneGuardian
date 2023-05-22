// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventories / InventorySlotUI.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameDevTV.Core.UI.Dragging;
using UnityEngine.InputSystem;
using TMPro;

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