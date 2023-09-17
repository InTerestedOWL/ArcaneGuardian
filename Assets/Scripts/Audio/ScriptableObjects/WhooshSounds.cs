using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Audio.Sounds {
    [CreateAssetMenu(fileName = "WhooshSounds", menuName = ("Arcane Guardian/Audio/Sound/Whooshes"))]
    public class WhooshSounds : ScriptableObject {
        [SerializeField]
        List<AudioClip> whooshSounds = new List<AudioClip>();

        public AudioClip GetRandomWhooshSound() {
            return whooshSounds[Random.Range(0, whooshSounds.Count)];
        }

        public bool HasWhooshSounds() {
            return whooshSounds.Count > 0;
        }
    }

}