using AG.Skills;
using UnityEngine;

public class Skillbook : MonoBehaviour {
    [SerializeField]
    GameObject skillbookCollectionUIReference;

    [SerializeField]
    Skill[] learnedSkills;

    private SkillbookUI sbUI;

    void Start() {
        sbUI = skillbookCollectionUIReference.GetComponent<SkillbookUI>();
        foreach (Skill skill in learnedSkills) {
            sbUI.AddToSkillbook(skill);
        }
    }

    public void AddSkill(Skill skill) {
        sbUI.AddToSkillbook(skill);
    }
}
