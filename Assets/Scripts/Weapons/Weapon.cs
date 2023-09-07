using UnityEngine;

namespace AG.Weapons {
    public class Weapon : MonoBehaviour {
        public enum WeaponType {
            OneHanded,
            TwoHanded,
            Bow,
            Staff
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
                    animator.SetBool("isBow", false);
                    break;
                case WeaponType.TwoHanded:
                    // TODO: Optional: Add two handed animations
                    animator.SetBool("isTwoHanded", true);
                    animator.SetBool("isOneHanded", false);
                    animator.SetBool("isBow", false);
                    break;
                case WeaponType.Bow:
                    animator.SetBool("isBow", true);
                    animator.SetBool("isTwoHanded", false);
                    animator.SetBool("isOneHanded", false);
                    break;
            }
        }

        public virtual void Use() {
        }

        
        public virtual void Use(GameObject target) {
        }

        public int GetAttackDmg() {
            return attackDamage;
        }

        public float GetKnockbackForce() {
            return knockbackForce;
        }
    }
}