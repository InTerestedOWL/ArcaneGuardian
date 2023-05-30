using AG.Skills;
using UnityEngine;
using UnityEngine.UI;

using AG.UI.Draggable;

public class SkillbookUI : MonoBehaviour {
    [SerializeField]
    public GameObject skillEntryUIPrefab;

    public void AddToSkillbook(Skill skill) {
        GameObject skillEntryUI = Instantiate(skillEntryUIPrefab, transform);
        AddSkillIconAndRef(skill, skillEntryUI);
        AddSkillTitle(skill, skillEntryUI); 
        AddSkillDescription(skill, skillEntryUI);  
    }

    private void AddSkillIconAndRef(Skill skill, GameObject skillEntryUI) {
        Transform iconContainer = skillEntryUI.transform.Find("Icon Container");
        iconContainer.GetChild(0).GetComponent<Image>().sprite = skill.GetIcon();
        iconContainer.GetChild(0).GetComponent<DragSkills>().SetCurrentAction(skill);
    }

    private void AddSkillTitle(Skill skill, GameObject skillEntryUI)
    {
        Transform title = skillEntryUI.transform.Find("Skill Title");
        title.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = skill.GetDisplayName();
    }

    private void AddSkillDescription(Skill skill, GameObject skillEntryUI)
    {
        Transform title = skillEntryUI.transform.Find("Skill Description");
        title.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = skill.GetDescription();
    }
}
