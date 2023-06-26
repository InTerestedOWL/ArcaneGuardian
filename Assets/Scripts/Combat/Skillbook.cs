using AG.Skills;
using UnityEngine;
using System.Collections.Generic;

public class Skillbook : MonoBehaviour {
    [SerializeField]
    GameObject skillbookCollectionUIReference;

    [SerializeField]
    List<Skill> learnedSkills;

    private SkillbookUI sbUI;

    void Start() {
        sbUI = skillbookCollectionUIReference.GetComponent<SkillbookUI>();
        foreach (Skill skill in learnedSkills) {
            sbUI.AddToSkillbook(skill);
        }
    }

    public void AddSkill(Skill skill) {
        learnedSkills.Add(skill);
        sbUI.AddToSkillbook(skill);
    }

    public void RemoveSkill(Skill skill) {
        learnedSkills.Remove(skill);
        sbUI.RemoveFromSkillbook(skill);
    }
}
