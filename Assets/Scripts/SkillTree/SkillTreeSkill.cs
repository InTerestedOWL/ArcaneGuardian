using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using AG.Skills;
using UnityEngine.EventSystems;

namespace AG.Talents {
    // TODO: Combine SkillTree Skill and SkillBook Skill?
    public class SkillTreeSkill : MonoBehaviour
    {
        [SerializeField]
        public SkillTree skillTree;
        [SerializeField]
        public int SkillCap = 1; //TODO: Remove if SkillCap is implemented in Skill.cs
        [SerializeField]
        public TMP_Text SkillNameText;
        [SerializeField]
        public TMP_Text SkillDescriptionText;
        [SerializeField]
        public TMP_Text SkillLevelText;
        [SerializeField]
        public Image SkillIcon;
        [SerializeField]
        public SkillTreeSkill[] ConnectedSkills;

        public Skill skill;

        private int SkillLevel = 0;
        private bool buyable = true;
        private Skillbook skillbook;

        public void Start() {
            skillbook = GameObject.Find("Player").GetComponent<Skillbook>();
        }

        public void UpdateUI() {
            SkillNameText.text = skill.GetDisplayName();
            SkillDescriptionText.text = skill.GetDescription();
            //TODO: Repalce if SkillCap is implemented in Skill.cs
            SkillLevelText.text = $"{SkillLevel}/{SkillCap}";
            // SkillLevelText.text = $"{SkillLevel}/{skill.GetSkillCap()}";
            SkillIcon.sprite = skill.GetIcon();

            if(!buyable){
                this.gameObject.GetComponent<Button>().enabled = false;
                this.gameObject.GetComponent<RightClickButton>().enabled = false;
            }
            else{
                this.gameObject.GetComponent<Button>().enabled = true;
                this.gameObject.GetComponent<RightClickButton>().enabled = true;
            }

            //TODO: Fix Coloring Bug
            GetComponent<Image>().color = SkillLevel >= SkillCap ? Color.yellow :
                skillTree.SkillPoints > 0 && buyable ? Color.green : Color.white;

            foreach(var connectedSkill in ConnectedSkills){
                if(SkillLevel > 0)
                    connectedSkill.SetBuyable(true);
                else
                    connectedSkill.SetBuyable(false);
            }
        }

        public void Learn() {
            if(skillTree.SkillPoints < 1 || SkillLevel >= SkillCap)
                return;

            if(SkillLevel == 0) {
                skillbook.AddSkill(skill);
            }

            skillTree.SkillPoints--;
            SkillLevel++;
            skillTree.UpdateAllSkillUI();
        }

        public void Unlearn() {
            if(SkillLevel == 0) {
                return;
            }
                
            foreach(SkillTreeSkill connectedSkill in ConnectedSkills){
                while(connectedSkill.SkillLevel > 0) {
                    connectedSkill.Unlearn();
                }
            }

            SkillLevel--;
            skillTree.SkillPoints++;

            if(SkillLevel == 0) {
                skillbook.RemoveSkill(skill);
            }
            
            skillTree.UpdateAllSkillUI();
        }

        public void SetBuyable(bool buyable) {
            this.buyable = buyable;
        }
    }
}