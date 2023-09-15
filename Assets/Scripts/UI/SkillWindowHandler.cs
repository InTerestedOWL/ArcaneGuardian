using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWindowHandler : MonoBehaviour {
    void OnEnable() {
        TutorialHandler.AddTutorialToShow("Skillbook", "TalentLearned");
    }
}
