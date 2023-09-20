using System.Collections;
using System.Collections.Generic;
using AG.Combat;
using Unity.VisualScripting;
using UnityEngine;

namespace AG.Wave {
    public class PlaneOfDeath : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other) {
            CombatTarget ct = other.GetComponent<CombatTarget>(); 
            
            if(ct != null) {
                ct.DamageTarget((int)ct.maxHealth + 1);
            }
        }
    }
}
