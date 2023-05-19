// Based on https://www.udemy.com/course/unityrpg/

using UnityEngine;

namespace AG.Combat {
    public class BasicCombat : MonoBehaviour {
        // Counter to check if attack is buffered
        private int attackStateCounter = 0;
        public void Attack() {
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
    }
}
