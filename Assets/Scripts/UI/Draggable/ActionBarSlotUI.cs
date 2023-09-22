// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventories / InventorySlotUI.cs

using UnityEngine;
using GameDevTV.Core.UI.Dragging;
using UnityEngine.InputSystem;
using TMPro;
using AG.Actions;
using UnityEngine.EventSystems;
using AG.Control;
using AG.Skills.Targeting;
using AG.Skills;

namespace AG.UI.Draggable {
    public class ActionBarSlotUI : MonoBehaviour, IDragContainer<ActionItem> {
        [SerializeField] 
        InputActionReference hotkeyReference = null;
        [SerializeField]
        InputActionReference castingReference = null;
        string hotkey = "";
        [SerializeField]
        SkillRef skillRef = null;

        [SerializeField] private TMP_Text infoBoxSpellName;

        [SerializeField] private TMP_Text infoBoxSpellDesc;

        [SerializeField] private TMP_Text infoBoxSpellCooldown;

        [SerializeField] private TMP_Text infoBoxSpellDamage;

        [SerializeField] private GameObject infoBox;

        private string containerName;

        private void Awake() {
            SetHotkeyUIDisplay();
        }

        private void Start() {
            hotkeyReference.action.started += TriggerHotkey;
            if(infoBox != null){
                infoBox.SetActive(false);
            }
            if (castingReference != null) {
                castingReference.action.started += TriggerHotkey;
            }
        }
        public void showInfoBox(bool b){
            if(infoBox != null && infoBoxSpellName.text != ""){
                infoBox.SetActive(b);
            }else if(infoBox != null){
                infoBox.SetActive(false);
            }
        }
        private void SetHotkeyUIDisplay() {
            containerName = this.gameObject.transform.parent.gameObject.transform.parent.transform.name;
            
            if (hotkeyReference != null) {
                // Takes only the first assigned key.
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
                     
                if(item is Skill){
                    Skill skill = (Skill)item;
                    infoBoxSpellCooldown.text = $"CD: {skill.GetMaxCooldown()}s";
                    infoBoxSpellDamage.text = $"DMG: {skill.GetDamage()}";
                }          
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
                infoBoxSpellName.text = "";
                infoBoxSpellDesc.text = "";
                infoBoxSpellCooldown.text = "";
                infoBoxSpellDamage.text = "";
            }
        }

        private void TriggerHotkey(InputAction.CallbackContext context) {
            if (TargetingStrategy.activeStrategies.Count > 0) {
                TargetingStrategy currentTargeting = TargetingStrategy.activeStrategies.Dequeue();
                currentTargeting.cancelTargeting = true;
            }
            skillRef.UseCurrentItem();
        }

        private void OnDestroy() {
            skillRef = null;
            hotkeyReference.action.started -= TriggerHotkey;
            if (castingReference != null)
            {
                castingReference.action.started -= TriggerHotkey;
            }
        }
    }
}