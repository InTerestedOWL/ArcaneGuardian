using UnityEngine;

namespace AG.Weapons {
    public class Weapon : MonoBehaviour {
        public enum WeaponType {
            OneHanded,
            TwoHanded
        }

        [SerializeField]
        public WeaponType currentWeaponType;

        [SerializeField]
        public int attackDamage = 10;

        [SerializeField]
        public float knockbackForce = 10;

        Animator animator;

        protected virtual void Start() {
            animator = GetComponentInParent<Animator>();
            ChangeWeaponType();
        }

        protected void SetCurWeaponType(WeaponType weaponType) {
            currentWeaponType = weaponType;
        }

        protected void ChangeWeaponType() {
            switch(currentWeaponType) {
                case WeaponType.OneHanded:
                    animator.SetBool("isOneHanded", true);
                    animator.SetBool("isTwoHanded", false);
                    break;
                case WeaponType.TwoHanded:
                    // TODO: Optional: Add two handed animations
                    animator.SetBool("isTwoHanded", true);
                    animator.SetBool("isOneHanded", false);
                    break;
            }
        }

        public int GetAttackDmg() {
            return attackDamage;
        }

        public float GetKnockbackForce() {
            return knockbackForce;
        }
    }
}