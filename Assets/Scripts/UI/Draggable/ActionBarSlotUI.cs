// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventories / InventorySlotUI.cs

using UnityEngine;
using GameDevTV.Core.UI.Dragging;
using UnityEngine.InputSystem;
using TMPro;
using AG.Actions;
using UnityEngine.EventSystems;

namespace AG.UI.Draggable {
    public class ActionBarSlotUI : MonoBehaviour, IDragContainer<ActionItem> {
        [SerializeField] 
        InputActionReference hotkeyReference = null;
        string hotkey = "";
        [SerializeField]
        SkillRef skillRef = null;

        [SerializeField] private TMP_Text infoBoxSpellName;

        [SerializeField] private TMP_Text infoBoxSpellDesc;

        [SerializeField] private TMP_Text infoBoxSpellCooldown;

        private string containerName;

        private void Awake() {
            SetHotkeyUIDisplay();
        }

        private void Start() {
            hotkeyReference.action.started += TriggerHotkey;
        }

        private void SetHotkeyUIDisplay() {
            
            containerName = this.gameObject.transform.parent.gameObject.transform.parent.transform.name;
            if(containerName == "Action Bar Container"){
                EventTrigger et = this.gameObject.GetComponent<EventTrigger>();
                et.enabled = false;
            }
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
            if(containerName == "Action Bar Container"){
                skillRef.SetItem(item);
                infoBoxSpellName.text = item.GetDisplayName();
                infoBoxSpellDesc.text = item.GetDescription();
                infoBoxSpellCooldown.text = item.GetMaxCooldown().ToString()+" Sec.";
                EventTrigger et = this.gameObject.GetComponent<EventTrigger>();
                et.enabled = true;
            }      
        }

        public ActionItem GetItem() {
            return skillRef.GetItem();       
        }

        public int GetNumber() {
            return 1;
        }

        public void RemoveItems(int number) {
            if(containerName == "Action Bar Container"){
                skillRef.SetItem(null);
                EventTrigger et = this.gameObject.GetComponent<EventTrigger>();
                et.enabled = false;
            }
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