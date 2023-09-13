using UnityEngine;

using AG.Weapons;
using AG.Audio.Sounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

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
        private CharacterAudioController audioController;

        private bool isDead = false;

        private void Start() {
            currentHealth = maxHealth;
            rb = GetComponent<Rigidbody>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            audioController = GetComponent<CharacterAudioController>();

            if (skinnedMeshRenderer == null){
                material = meshRenderer.material;
            }
            else {
                material = skinnedMeshRenderer.material;
            }

            defaultColor = material.color;
        }

        private void FixedUpdate() {
            if (isKnockbacked && !isBuilding && rb != null) {
                HandleKnockBack(knockbackForce, knockbackDirection);
            }

            BlinkOnDamage();
        }

        private void OnTriggerEnter(Collider other) {
            if ((other.tag == "PlayerWeapon" && tag == "Enemy" || 
                other.tag == "EnemyWeapon" && tag == "Player" || 
                other.tag == "EnemyWeapon" && tag == "POI" || 
                other.tag == "EnemyWeapon" && tag == "Turret") 
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
                TakeDamage(weapon.GetAttackDmg(), other.gameObject);
            }
        }

        private void TakeDamage(int damage, GameObject sourceObject = null) {
            if (isDead)
                return;
            PlayHitSound(sourceObject);
            currentHealth -= damage;
            healthBar?.SetHealthBarPercentage(currentHealth / maxHealth);
            if (currentHealth <= 0) {
                Die();
            } else if (audioController != null) {
                audioController.PlayRandomPainSound();
            }

            blinkTimer = blinkDuration;
        }

        private void TakeHeal(int heal) {
            if (isDead)
                return;

            if(currentHealth + heal <= maxHealth) {
                currentHealth += heal;
            } else {
                currentHealth = maxHealth;
            }

            healthBar?.SetHealthBarPercentage(currentHealth / maxHealth);
        }

        private void PlayHitSound(GameObject sourceObject = null) {
            List<AudioClip> hitSounds = new List<AudioClip>();
            if (sourceObject != null) {
                // Search for CharacterAudioController in parent of sourceObject
                // Can be used to play different impact sounds for different weapons or characteristics of the other CombatTarget
                CharacterAudioController controller = sourceObject.GetComponentInParent<CharacterAudioController>();
                if (controller != null) {
                    hitSounds = controller.GetHitSoundsForTarget();
                }
            }
            if (audioController != null) {
                audioController.PlayRandomHitSound(hitSounds);
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
            // rb.isKinematic = true;
            healthBar?.gameObject.SetActive(false);
            isDead = true;

            if (audioController != null) {
                audioController.PlayRandomDeathSound();
            }

            if(tag == "Enemy") {
                GetComponent<EnemyResources>().onDie();
                if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
                {
                    WaveSpawner ws = GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>();
                    ws.spawnedEnemies.Remove(gameObject);
                    ws.updateEnemiesAliveUI();
                }

                //Despawn Enemies on Death
                StartCoroutine(DespawnOnDeath());
            }

            if(isBuilding) {
                GetComponent<NavMeshObstacle>().enabled = false;

                //Originales GameObjekt wird in DestroyMesh zerstört.
                GetComponent<MeshDestroy>()?.DestroyMesh();

                foreach(MeshDestroy meshDestroy in GetComponentsInChildren<MeshDestroy>()) {
                    meshDestroy.DestroyMesh();
                }

                PlaceableObject po = GetComponent<PlaceableObject>();
                BuildingSystem bs = GameObject.Find("Grid").GetComponent<BuildingSystem>();
                if(po != null) {
                    Vector3Int start = bs.gridLayout.WorldToCell(po.GetStartPosition());
                    bs.tileToPlacable(start, po.Size);
                    bs.poi_building.removePlacedBuilding(po);
                }
                
            }
        }

        private IEnumerator DespawnOnDeath() {
            float sinkSpeed = 0.5f;
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            Animator animator = GetComponent<Animator>();

            //Wait for Death Animation to finish
            while(!animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) {
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(3f);
            
             //Translate Enemy into Ground
            while (transform.position.y > -1.5f) {
                transform.Translate(Vector3.down * sinkSpeed *  Time.deltaTime);
                yield return null;
            }

            //Despawn Enemies on Death
            Destroy(gameObject, 1f);
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

        public void HealTarget(int heal) {
            //TODO: Add Heal Animation/Effect
            TakeHeal(heal);
        }

        public void DamageTargetDoTInAoE(int damagePerTick, float numberOfTicksInDuration, float duration) {
            if (coroutineAoEDoT == null) {
                coroutineAoEDoT = StartCoroutine(DealDamageOverTimeInAoE(damagePerTick, numberOfTicksInDuration, duration));
            }
        }

        public void HealTargetHoTInAoE(int healingPerTick, float numberOfTicksInDuration, float duration) {
            coroutineAoEDoT = StartCoroutine(DealHealingOverTimeInAoE(healingPerTick, numberOfTicksInDuration, duration));
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

        private IEnumerator DealHealingOverTimeInAoE(int healingPerTick, float numberOfTicksInDuration, float duration) {
            inAoERange = true;
            for (int i = 0; i < numberOfTicksInDuration; i++) {
                if (inAoERange) {
                    TakeHeal(healingPerTick);
                }
                yield return new WaitForSeconds(duration / numberOfTicksInDuration);
            }
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
