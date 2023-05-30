using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Skill : MonoBehaviour
{
    [SerializeField]
    public SkillTree skillTree;
    [SerializeField]
    public string SkillName = "SkillName";
    [SerializeField]
    public string SkillDescription = "SkillDescription";
    [SerializeField]
    public int SkillCap = 1;
    [SerializeField]
    public TMP_Text SkillNameText;
    [SerializeField]
    public TMP_Text SkillDescriptionText;
    [SerializeField]
    public TMP_Text SkillLevelText;
    [SerializeField]
    public Skill[] ConnectedSkills;

    private int SkillLevel = 0;
    private bool buyable = true;

    public void UpdateUI(){
        SkillNameText.text = $"{SkillName}";
        SkillDescriptionText.text = $"{SkillDescription}";
        SkillLevelText.text = $"{SkillLevel}/{SkillCap}";

        if(!buyable){
            this.gameObject.GetComponent<Button>().enabled = false;
        }
        else{
            this.gameObject.GetComponent<Button>().enabled = true;
        }

        GetComponent<Image>().color = SkillLevel >= SkillCap ? Color.yellow :
            skillTree.SkillPoints > 0 && buyable ? Color.green : Color.grey;

        foreach(var connectedSkill in ConnectedSkills){
            if(SkillLevel > 0)
                connectedSkill.SetBuyable(true);
            else
                connectedSkill.SetBuyable(false);
        }
    }

    public void Buy(){
        if(skillTree.SkillPoints < 1 || SkillLevel >= SkillCap)
            return;

        skillTree.SkillPoints--;
        SkillLevel++;
        skillTree.UpdateAllSkillUI();
    }

    public void SetBuyable(bool buyable) {
        this.buyable = buyable;
    }
}
