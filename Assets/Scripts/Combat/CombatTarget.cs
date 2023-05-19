using UnityEngine;

using AG.Weapons;

namespace AG.Combat {
    public class CombatTarget : MonoBehaviour {
        [SerializeField]
        public int currentHealth = 100;
        private bool isKnockbacked = false;
        private Vector3 knockbackDirection = Vector3.zero;
        private float knockbackForce = 0f;
        private Rigidbody rb;

        private void Start() {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            if (isKnockbacked) {
                HandleKnockBack(knockbackForce, knockbackDirection);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if ((other.tag == "PlayerWeapon" && tag == "Enemy" || other.tag == "EnemyWeapon" && tag == "Player") 
                && other.GetComponentInParent<BasicCombat>().IsAttacking()) {
                HandleHit(other);
            }
        }

        private void HandleHit(Collider other) {
            if (currentHealth > 0) {
                Weapon weapon = other.GetComponent<Weapon>();
                knockbackDirection = transform.position - other.transform.root.position;
                knockbackForce = weapon.GetKnockbackForce();
                isKnockbacked = true;
                TakeDamage(weapon.GetAttackDmg());
            }
        }

        private void TakeDamage(int damage) {
            currentHealth -= damage;
            if (currentHealth <= 0) {
                Die();
            }
        }

        private void HandleKnockBack(float force, Vector3 knockbackDirection) {
            isKnockbacked = false;
            // Add a small force in y direction to prevent the enemy from getting stuck in the ground
            knockbackDirection.y = 0.02f * force;
            rb.AddForce(knockbackDirection * force, ForceMode.Impulse);
        }

        private void Die() {
            GetComponent<Animator>().SetTrigger("killed");
            rb.isKinematic = true;
        }
    }
}
