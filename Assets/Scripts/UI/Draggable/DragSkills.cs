// Based on code from:
// GameDev.tv Assets / Scripts / Ui / Inventories / InventoryDragItem.cs

using System.Collections;
using System.Collections.Generic;
using AG.Actions;
using UnityEngine;

namespace AG.UI.Draggable {
    public class DragSkills : DragItem<ActionItem> {
        ActionItem actionItem = null;

        public void SetCurrentAction(ActionItem action) {
            actionItem = action;
        }

        public ActionItem GetCurrentAction() {
            return actionItem;
        }
    }
}