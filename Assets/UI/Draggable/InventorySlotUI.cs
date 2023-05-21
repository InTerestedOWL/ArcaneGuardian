// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventorioes / InventorySlotUI.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameDevTV.Core.UI.Dragging;

namespace AG.UI.Draggable {
    public class InventorySlotUI : MonoBehaviour, IDragContainer<Sprite> {
        [SerializeField] InventoryItemIcon icon = null;
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