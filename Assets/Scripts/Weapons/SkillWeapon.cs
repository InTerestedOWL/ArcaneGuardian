using AG.Skills;
using UnityEngine;

namespace AG.Weapons {
    public class SkillWeapon : Weapon {
        [SerializeField]
        AiSkill bowAttackSkill;

        public override void Use() {
            bowAttackSkill.Use(this.gameObject);
        }

        public override void Use(GameObject target) {
            bowAttackSkill.Use(this.gameObject, target);
        }
    }
}