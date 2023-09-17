using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using AG.Skills;
using UnityEngine.EventSystems;

namespace AG.UI {
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
        public TMP_Text SkillDamageText;
        [SerializeField]
        public TMP_Text SkillCooldownText;
        [SerializeField]
        public Image SkillIcon;
        [SerializeField]
        public SkillTreeSkill[] ConnectedSkills;

        public Skill skill;

        private int SkillLevel = 0;
        private bool buyable = true;
        private Skillbook skillbook;

        private Image backgroundImage;
        private Image skillImage; 
        private Button button;
        private ColorBlock buttonColors;
        private GlobalAudioSystem globalAudioSystem;

        public void Start() {
            skillbook = GameObject.Find("Player").GetComponent<Skillbook>();
            backgroundImage = GetComponent<Image>();
            skillImage = this.transform.GetChild(0).GetComponent<Image>();
            skillImage.GetComponent<Image>().material = Instantiate(skillImage.GetComponent<Image>().material);
            button = GetComponent<Button>();
            buttonColors = button.colors;
            globalAudioSystem = GameObject.Find("Main Camera").GetComponent<GlobalAudioSystem>();

            UpdateUI();
        }

        public void UpdateUI() {
            SkillNameText.text = skill.GetDisplayName();
            SkillDescriptionText.text = skill.GetDescription();
            SkillLevelText.text = $"Level {SkillLevel}/{skill.GetSkillCap()}";
            SkillDamageText.text = $"DMG: {skill.GetDamage()}";
            SkillCooldownText.text = $"CD: {skill.GetMaxCooldown()}s"; 
            SkillIcon.sprite = skill.GetIcon();

            if(!buyable){
                this.gameObject.GetComponent<Button>().enabled = false;
                this.gameObject.GetComponent<RightClickButton>().enabled = false;
            }
            else{
                this.gameObject.GetComponent<Button>().enabled = true;
                this.gameObject.GetComponent<RightClickButton>().enabled = true;
            }

            foreach(var connectedSkill in ConnectedSkills){
                if(SkillLevel > 0)
                    connectedSkill.SetBuyable(true);
                else
                    connectedSkill.SetBuyable(false);
            }

            Debug.Log(buttonColors);
            Debug.Log(backgroundImage);
            if(SkillLevel >= SkillCap) {
                buttonColors.normalColor = Color.yellow;
                backgroundImage.color = Color.yellow;
            }
            else {
                if(skillTree.SkillPoints > 0 && buyable) {
                    buttonColors.normalColor = Color.green;
                    backgroundImage.color = Color.green;
                }
                else {
                    buttonColors.normalColor = Color.white;
                    backgroundImage.color = Color.white;
                }
            } 

            button.colors = buttonColors;

            if(SkillLevel > 0){
                skillImage.material.SetFloat("_GrayscaleAmount", 0f);
            }
            else {
                skillImage.material.SetFloat("_GrayscaleAmount", 1f);
            }
        }

        public void Learn() {
            if(skillTree.SkillPoints < 1 || SkillLevel >= SkillCap) {
                return;
            }

            if (globalAudioSystem) {
                globalAudioSystem.PlayUIClickSound();
            }

            if (SkillLevel == 0) {
                TutorialHandler.AddTutorialToShow("TalentLearned", "TalentsOpen");
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

            if (globalAudioSystem) {
                globalAudioSystem.PlayUIDeclineSound();
            }

            foreach (SkillTreeSkill connectedSkill in ConnectedSkills){
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