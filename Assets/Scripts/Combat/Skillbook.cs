using AG.Skills;
using UnityEngine;
using System.Collections.Generic;
using AG.UI.Draggable;
using AG.Actions;

public class Skillbook : MonoBehaviour {
    [SerializeField]
    GameObject skillbookCollectionUIReference;

    [SerializeField]
    List<Skill> learnedSkills;

    private SkillbookUI sbUI;
    private GameObject actionBarReference;
    void Start() {
        sbUI = skillbookCollectionUIReference.GetComponent<SkillbookUI>();
        foreach (Skill skill in learnedSkills) {
            sbUI.AddToSkillbook(skill);
        }

        actionBarReference = GameObject.Find("Action Bar Window");
    }

    public void AddSkill(Skill skill) {
        learnedSkills.Add(skill);
        sbUI.AddToSkillbook(skill);
    }

    public void RemoveSkill(Skill skill) {
        learnedSkills.Remove(skill);
        sbUI.RemoveFromSkillbook(skill);

        //Remove skill from actionbar
        ActionBarSlotUI[] actionBarSlotUIs = actionBarReference.GetComponentsInChildren<ActionBarSlotUI>();
        foreach (ActionBarSlotUI actionBarSlotUI in actionBarSlotUIs) {
            ActionItem actionItem = actionBarSlotUI.GetItem();
            if (actionItem?.GetDisplayName() == skill.GetDisplayName()) {
                actionBarSlotUI.RemoveItems(0); //Warum muss ich bei der Methode einen int angeben?
            }
        }
    }
}
