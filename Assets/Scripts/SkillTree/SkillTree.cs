using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTree : MonoBehaviour
{
    public List<Skill> SkillList;
    public GameObject SkillHolder;

    [SerializeField]
    public TMP_Text SkillPointsText;

    //Number of Skill Points the Players has to spend on his Skills
    public int SkillPoints;

    private void Start() {
        SkillPoints = 20;
        SkillPointsText.text = SkillPoints.ToString();

        foreach(var skill in SkillHolder.GetComponentsInChildren<Skill>()){
            SkillList.Add(skill);
        }

        UpdateAllSkillUI();
    }

    public void UpdateAllSkillUI(){
        foreach(var skill in SkillList){
            skill.UpdateUI();
            SkillPointsText.text = SkillPoints.ToString();
        }
    }
}
