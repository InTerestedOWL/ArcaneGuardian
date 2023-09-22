using AG.Weapons;
using UnityEngine;
using UnityEngine.VFX;

namespace AG.Combat {
    public class BasicCombat : MonoBehaviour {
        [SerializeField]
        VisualEffect slashEffect;
        // Counter to check if attack is buffered
        private int attackStateCounter = 0;

        public void Attack() {
            if (!IsAttacking())
                UpdateAnimator();
        }

        public void IncreaseAttackStateCounter() {
            attackStateCounter++;
        }

        public void DecreaseAttackStateCounter() {
            attackStateCounter--;
        }

        public bool IsAttacking() {
            return attackStateCounter > 0;
        }

        private void UpdateAnimator() {
            GetComponent<Animator>().SetTrigger("basicAttack");
        }

        public void PlaySlashEffect()
        {
            if(slashEffect == null)
                return;

            if (attackStateCounter == 1)
            {
                slashEffect.gameObject.SetActive(true);
                slashEffect.SendEvent("SlashStart");
            }
        }

        public void StopSlashEffect()
        {
            if(slashEffect == null)
                return;

            slashEffect.gameObject.SetActive(false);
            slashEffect.SendEvent("SlashStop");
        }
    }
}
