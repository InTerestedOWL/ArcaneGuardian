using UnityEngine;

using AG.Weapons;

namespace AG.Combat {
    public class CombatTarget : MonoBehaviour {
        [SerializeField]
        public float maxHealth = 100;
        public float currentHealth;
        public float blinkDuration = 0.05f;
        public float blinkIntensity = 5.0f;
        public Color blinkColor = Color.white;

        private bool isKnockbacked = false;
        private Vector3 knockbackDirection = Vector3.zero;
        private float knockbackForce = 0f;
        private Rigidbody rb;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private float blinkTimer;
        private Color defaultColor;
        private HealthBarUI healthBar;

        private void Start() {
            currentHealth = maxHealth;
            rb = GetComponent<Rigidbody>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            defaultColor = skinnedMeshRenderer.material.color;
            healthBar = GetComponentInChildren<HealthBarUI>();
        }

        private void FixedUpdate() {
            if (isKnockbacked) {
                HandleKnockBack(knockbackForce, knockbackDirection);
            }

            BlinkOnDamage();
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
            healthBar?.SetHealthBarPercentage(currentHealth / maxHealth);
            if (currentHealth <= 0) {
                Die();
            }

            blinkTimer = blinkDuration;
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
            healthBar?.gameObject.SetActive(false);
        }

        //Blink Animation on Damage taken
        private void BlinkOnDamage(){
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntensity) + 1.0f;
            
            if(intensity > 1.0f){
                skinnedMeshRenderer.material.color = blinkColor * intensity;
            }
            else {
                skinnedMeshRenderer.material.color = defaultColor;
            }
        }
    }
}
