// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventories / InventoryDragItem.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.UI.Draggable {
    public class DragSkillFromSpellbook : DragItem<Sprite> {
        private void Awake() {
            keepSource = true;
        }
    }
}