// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventories / InventorySlotUI.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameDevTV.Core.UI.Dragging;
using AG.Actions;

namespace AG.UI.Draggable {
    public class SkillbookSlotUI : MonoBehaviour, IDragContainer<ActionItem> {
        [SerializeField] SkillRef skillRef = null;
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
            // CUSTOMIZED: Don't remove the item from the slot.
        }
    }
}