using UnityEngine;

public class CloseMenu : MonoBehaviour {
    public void CloseMenuAction() {
        GetComponentInParent<ToggleMenu>().CloseMenu();
    }
}
