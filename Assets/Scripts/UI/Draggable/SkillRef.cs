// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventories / InventoryItemIcon.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AG.Actions;

namespace AG.UI.Draggable {
    /// <summary>
    /// To be put on the icon representing an inventory item. Allows the slot to
    /// update the icon and number.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SkillRef : MonoBehaviour {
        ActionItem curItem = null;

        public void Start() {
            DragSkills dragItem = GetComponent<DragSkills>();
            curItem = dragItem.GetCurrentAction();
        }

        public void SetItem(ActionItem item) {
            var iconImage = GetComponent<Image>();
            if (curItem != null) {
                curItem.RemoveCooldownUI(transform.parent.GetComponentInChildren<SlotCooldownUI>());
            }
            if (item == null) {
                iconImage.enabled = false;
                curItem = null;
            } else {
                iconImage.sprite = item.GetIcon();
                iconImage.enabled = true;
                curItem = item;
                curItem.AddNewCooldownUI(transform.parent.GetComponentInChildren<SlotCooldownUI>());
            }
        }

        public void UseCurrentItem() {
            if (curItem != null) {
                GameObject player = GameObject.FindWithTag("Player");
                curItem.Use(player);
            }
        }

        public ActionItem GetItem() {
            var iconImage = GetComponent<Image>();
            if (!iconImage.enabled) {
                return null;
            }
            return curItem;
        }
    }
}