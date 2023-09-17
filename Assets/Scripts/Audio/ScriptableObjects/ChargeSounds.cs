using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Audio.Sounds
{
    [CreateAssetMenu(fileName = "ChargeSounds", menuName = ("Arcane Guardian/Audio/Sound/Charges"))]
    public class ChargeSounds : ScriptableObject
    {
        [SerializeField]
        List<AudioClip> chargeSounds = new List<AudioClip>();

        public AudioClip GetRandomChargeSound() {
            return chargeSounds[Random.Range(0, chargeSounds.Count)];
        }

        public List<AudioClip> GetChargeSounds()
        {
            return chargeSounds;
        }

        public bool HasChargeSounds()
        {
            return chargeSounds.Count > 0;
        }
    }

}