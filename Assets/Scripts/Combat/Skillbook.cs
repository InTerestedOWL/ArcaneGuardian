using AG.Skills;
using UnityEngine;

public class Skillbook : MonoBehaviour {
    [SerializeField]
    GameObject skillbookCollectionUIReference;

    [SerializeField]
    Skill[] learnedSkills;

    void Start() {
        SkillbookUI sbUI = skillbookCollectionUIReference.GetComponent<SkillbookUI>();
        foreach (Skill skill in learnedSkills) {
            sbUI.AddToSkillbook(skill);
        }
    }
}
