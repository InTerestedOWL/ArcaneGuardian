using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour {
    [SerializeField]
    private GameObject tabsParent;
    [SerializeField]
    private GameObject tabContentArea;
    [SerializeField]
    private ScreenResolutionSelector screenResSel;
    [SerializeField]
    private SoundSettings soundSettings;

    // Start is called before the first frame update
    void Start() {
        // Preselect toggle, as simply setting it "isOn" won't highlight it
        Toggle[] toggles = tabsParent.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles) {
            if (toggle.isOn) {
                //toggle.Select();
            }
        }
    }

    public void ChangeTab(Toggle toggle) {
        if (toggle.isOn)
            CheckTabState();
        ToggleContent(toggle);
    }

    private void ToggleContent(Toggle toggle) {
        try {
            GameObject content = tabContentArea.transform.Find(toggle.transform.name + " Content").gameObject;
            if (toggle.isOn) {
                content.SetActive(true);
            } else {
                content.SetActive(false);
            }
        } catch (System.Exception e) {
            Debug.Log(e);
        }
    }

    private void CheckTabState() {
        Toggle[] toggles = tabsParent.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles) {
            GameObject selectedIndicator = toggle.gameObject.transform.GetChild(2).gameObject;
            if (toggle.isOn) {
                selectedIndicator.SetActive(true);
            } else {
                selectedIndicator.SetActive(false);
            }
        }
    }

    public void CloseSettings() {
        screenResSel.Revert();
        soundSettings.Revert();
        gameObject.SetActive(false);
    }

    public void OpenSettings() {
        gameObject.SetActive(true);
    }
}
