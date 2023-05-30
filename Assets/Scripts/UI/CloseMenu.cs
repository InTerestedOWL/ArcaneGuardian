using UnityEngine;

namespace AG.UI {
    public class CloseMenu : MonoBehaviour {
        public void CloseMenuAction() {
            GetComponentInParent<ToggleMenu>().CloseMenu();
        }
    }
}
