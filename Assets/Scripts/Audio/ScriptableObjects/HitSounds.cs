using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Audio.Sounds
{
    [CreateAssetMenu(fileName = "HitSounds", menuName = ("Arcane Guardian/Audio/Sound/Hits"))]
    public class HitSounds : ScriptableObject
    {
        [SerializeField]
        List<AudioClip> hitSounds = new List<AudioClip>();

        public AudioClip GetRandomHitSound()
        {
            return hitSounds[Random.Range(0, hitSounds.Count)];
        }

        public List<AudioClip> GetHitSounds()
        {
            return hitSounds;
        }

        public bool HasHitSounds()
        {
            return hitSounds.Count > 0;
        }
    }

}