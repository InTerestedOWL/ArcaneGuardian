using UnityEngine;

using AG.Weapons;
using System.Collections;

namespace AG.Combat {
    public class CombatTarget : MonoBehaviour {

        public bool isBuilding = false;
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
        private MeshRenderer meshRenderer;
        private Material material;
        private float blinkTimer;
        private Color defaultColor;
        private HealthBarUI healthBar;
        private bool inAoERange = false;
        private Coroutine coroutineAoEDoT = null;

        private bool isDead = false;

        private void Start() {
            currentHealth = maxHealth;
            rb = GetComponent<Rigidbody>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            meshRenderer = GetComponentInChildren<MeshRenderer>();

            if(skinnedMeshRenderer == null){
                material = meshRenderer.material;
            }
            else {
                material = skinnedMeshRenderer.material;
            }

            defaultColor = material.color;
        }

        private void FixedUpdate() {
            if (isKnockbacked && !isBuilding) {
                HandleKnockBack(knockbackForce, knockbackDirection);
            }

            BlinkOnDamage();
        }

        private void OnTriggerEnter(Collider other) {
            if ((other.tag == "PlayerWeapon" && tag == "Enemy" || other.tag == "EnemyWeapon" && tag == "Player" || other.tag == "EnemyWeapon" && tag == "POI") 
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
            if (isDead)
                return;
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
            isDead = true;
        }

        //Blink Animation on Damage taken
        private void BlinkOnDamage(){
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntensity) + 1.0f;
            
            if(intensity > 1.0f){
                material.color = blinkColor * intensity;
            }
            else {
                material.color = defaultColor;
            }
        }

        public void DamageTarget(int damage) {
            TakeDamage(damage);
        }

        public void DamageTargetDoTInAoE(int damagePerTick, float numberOfTicksInDuration, float duration) {
            if (coroutineAoEDoT == null) {
                coroutineAoEDoT = StartCoroutine(DealDamageOverTimeInAoE(damagePerTick, numberOfTicksInDuration, duration));
            }
        }

        private IEnumerator DealDamageOverTimeInAoE(int damagePerTick, float numberOfTicksInDuration, float duration) {
            inAoERange = true;
            for (int i = 0; i < numberOfTicksInDuration; i++) {
                if (inAoERange) {
                    TakeDamage(damagePerTick);
                }
                yield return new WaitForSeconds(duration / numberOfTicksInDuration);
            }
            coroutineAoEDoT = null;
        }

        public void SetInAoERange(bool inAoERange) {
            this.inAoERange = inAoERange;
        }

        public void SetHealthBar(HealthBarUI healthBar){
            this.healthBar = healthBar;
        }

        public bool IsDead() {
            return isDead;
        }
    }
}
