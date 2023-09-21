using AG.Skills;
using UnityEngine;

namespace AG.Weapons {
    public class SkillWeapon : Weapon {
        [SerializeField]
        AiSkill weaponAttackSkill;

        public override void Use() {
            weaponAttackSkill.Use(this.gameObject);
        }

        public override void Use(GameObject target) {
            weaponAttackSkill.Use(this.gameObject, target);
        }
    }
}