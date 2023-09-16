using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AG.Audio.Sounds {
    public class CharacterAudioController : MonoBehaviour {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioSource additionalAudioSource;
        [SerializeField]
        private FootstepSounds footstepSounds;
        [SerializeField]
        private WhooshSounds whooshSounds;
        [SerializeField]
        private VoiceSounds voiceSounds;
        // Ex. Sounds for impact with own weapon (punches, sword, axe, etc.)
        [SerializeField]
        private HitSounds hitSoundsForTarget;
        // Ex. Sounds for impact on self (bones, etc)
        [SerializeField]
        private HitSounds hitSoundsForSelf;
        [SerializeField]
        private float runningThreshold = 3f;
        // To check for running, we need to know the speed of the character (blend tree value)
        private Animator animator;

        public enum FootstepType
        {
            Walking,
            Running
        }

        void Awake() {
            if (audioSource == null) {
                audioSource = GetComponent<AudioSource>();  
            }
            animator = GetComponent<Animator>();
        }

        void Start() {
            
        }

        void Update() {
            
        }

        public void PlaySound(AudioClip audioClip) {
            audioSource.PlayOneShot(audioClip);
        }

        private void PlayAdditionalSound(AudioClip audioClip) {
            additionalAudioSource.PlayOneShot(audioClip);
        }

        public void PlayRandomFootstepSound(FootstepType type) {
            float walkSpeed = Math.Abs(animator.GetFloat("walkSpeed"));
            float sideSpeed = Math.Abs(animator.GetFloat("sideSpeed"));
            if (sideSpeed <= walkSpeed) {
                if (type == FootstepType.Running && (walkSpeed >= runningThreshold)) {
                    PlaySound(footstepSounds.GetRandomRunningFootstepSound());
                } else if (type == FootstepType.Walking && (walkSpeed < runningThreshold)) {
                    PlaySound(footstepSounds.GetRandomWalkingFootstepSound());
                }
            }
        }

        public void PlayRandomFootstepStrafeSound(FootstepType type) {
            float walkSpeed = Math.Abs(animator.GetFloat("walkSpeed"));
            float sideSpeed = Math.Abs(animator.GetFloat("sideSpeed"));
            if (sideSpeed > walkSpeed) {
                if (type == FootstepType.Running && (animator.GetFloat("walkSpeed") >= runningThreshold)) {
                    PlaySound(footstepSounds.GetRandomRunningFootstepSound());
                } else if (type == FootstepType.Walking && (animator.GetFloat("walkSpeed") < runningThreshold)) {
                    PlaySound(footstepSounds.GetRandomWalkingFootstepSound());
                }
            }
        }

        public void PlayRandomWhooshSound() {
            if (whooshSounds != null)
                PlaySound(whooshSounds.GetRandomWhooshSound());
        }

        public void PlayRandomPainSound() {
            if (voiceSounds != null)
                PlaySound(voiceSounds.GetRandomPainSound());
        }

        public void PlayRandomDeathSound() {
            if (voiceSounds != null)
                PlaySound(voiceSounds.GetRandomDeathSound());
        }

        public void PlayAdditionalHitSound(HitSounds hitSounds) {
            if (hitSounds != null)
                PlayAdditionalSound(hitSounds.GetRandomHitSound());
        }

        // hitSoundsFromCombatTarget includes weapon impact sounds from the target that hits
        public void PlayRandomHitSound(List<AudioClip> hitSoundsFromCombatTarget) {
            List<AudioClip> ownHitSounds;
            if (hitSoundsForSelf == null) {
                ownHitSounds = new List<AudioClip>();
            } else {
                ownHitSounds = hitSoundsForSelf.GetHitSounds();
            }
            List<AudioClip> hitSounds = ownHitSounds.Concat(hitSoundsFromCombatTarget).ToList();
            if (hitSounds.Count > 0) {
                PlaySound(hitSounds[UnityEngine.Random.Range(0, hitSounds.Count)]);
            }
        }

        public List<AudioClip> GetHitSoundsForTarget() {
            if (hitSoundsForTarget == null)
                return new List<AudioClip>();
            return hitSoundsForTarget.GetHitSounds();
        }
    }
}