using UnityEngine;
using UnityEngine.UI;

public class SlotCooldownUI : MonoBehaviour {
    private Image cooldownImg = null;

    public void Start() {
        cooldownImg = GetComponent<Image>();
        cooldownImg.fillAmount = 0;
    }

    public void SetCooldown(float curCooldown, float maxCooldown) {
        cooldownImg.fillAmount = curCooldown / maxCooldown;
    }
}
