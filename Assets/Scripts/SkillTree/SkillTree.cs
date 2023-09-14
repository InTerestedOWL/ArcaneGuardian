using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AG.UI
{
    public class SkillTree : MonoBehaviour
    {
        public List<SkillTreeSkill> SkillList;
        public GameObject SkillHolder;

        [SerializeField]
        public TMP_Text SkillPointsText;

        //Number of Skill Points the Players can to spend on his Skills
        public int SkillPoints;

        private void Start()
        {
            SkillPoints = 20;
            SkillPointsText.text = SkillPoints.ToString();

            foreach (var skill in SkillHolder.GetComponentsInChildren<SkillTreeSkill>())
            {
                SkillList.Add(skill);
            }
        }

        private void OnEnable()
        {
            UpdateAllSkillUI();
            TutorialHandler.AddTutorialToShow("TalentsOpen", "Talents");
        }

        public void UpdateAllSkillUI()
        {
            SkillPointsText.text = SkillPoints.ToString();
            foreach (var skill in SkillList)
            {
                skill.UpdateUI();
            }
        }
    }

}