using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Audio.Sounds {
    [CreateAssetMenu(fileName = "FootstepSounds", menuName = ("Arcane Guardian/Audio/Sound/Footsteps"))]
    public class FootstepSounds : ScriptableObject {
        [SerializeField]
        List<AudioClip> runningFootstepSounds = new List<AudioClip>();

        [SerializeField]
        List<AudioClip> walkingFootstepSounds = new List<AudioClip>();

        public AudioClip GetRandomRunningFootstepSound() {
            return runningFootstepSounds[Random.Range(0, runningFootstepSounds.Count)];
        }

        public AudioClip GetRandomWalkingFootstepSound() {
            return walkingFootstepSounds[Random.Range(0, walkingFootstepSounds.Count)];
        }
    }

}