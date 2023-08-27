using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Audio.Sounds {
    [CreateAssetMenu(fileName = "VoiceSounds", menuName = ("Arcane Guardian/Audio/Sound/Voices"))]
    public class VoiceSounds : ScriptableObject {
        [SerializeField]
        List<AudioClip> painSounds = new List<AudioClip>();

        [SerializeField]
        List<AudioClip> deathSounds = new List<AudioClip>();

        public AudioClip GetRandomPainSound() {
            return painSounds[Random.Range(0, painSounds.Count)];
        }

        public AudioClip GetRandomDeathSound() {
            return deathSounds[Random.Range(0, deathSounds.Count)];
        }
    }

}