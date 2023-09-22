using UnityEngine;

namespace AG.UI {
    public class CloseMenu : MonoBehaviour {
        public void CloseMenuAction(GameObject menu) {
            GetComponentInParent<ToggleMenu>().CloseMenu(menu);
        }
    }
}
