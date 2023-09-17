using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AG.Audio.Sounds {
    public class SpellAudioController : MonoBehaviour {
        [SerializeField]
        private AudioSource audioSourceLoop;
        [SerializeField]
        private AudioSource audioSourceOneShot;
        [SerializeField]
        private AudioClip loopingSound;
        [SerializeField]
        private HitSounds groundHitSounds;
        [SerializeField]
        private ChargeSounds chargeSound;
        [SerializeField]
        private float timer = 0f;
        [SerializeField]
        private bool loopGroundHits = false;
        private bool stopSounds = false;
        private bool impacted = false;
        private Vector3 lastPos = Vector3.zero;

        void Start() {
            if (chargeSound) {
                PlaySound(chargeSound.GetRandomChargeSound());
            }
            if (timer > 0) {
                StartCoroutine(TimerStart());
            }
            PlayLoop();
            if (groundHitSounds && loopGroundHits) {
                StartCoroutine(PlayGroundHitSounds());
            }
        }

        void Update() {
            if (!impacted) {
                if (transform.position == lastPos) {
                    PlayRandomHitSound();
                    impacted = true;
                }
                lastPos = transform.position;
            }
        }

        private IEnumerator PlayGroundHitSounds() {
            while (groundHitSounds && !stopSounds) {
                yield return new WaitForSeconds(0.2f);
                PlayRandomHitSound();
            }
        }


        private IEnumerator TimerStart() {
            yield return new WaitForSeconds(timer);
            stopSounds = true;
            audioSourceLoop.Stop();
        }

        private void PlayLoop() {
            if (loopingSound) {
                audioSourceLoop.clip = loopingSound;
                audioSourceLoop.loop = true;
                audioSourceLoop.Play();
            }
        }

        private void PlaySound(AudioClip audioClip) {
            audioSourceOneShot.PlayOneShot(audioClip);
        }

        public void PlayRandomHitSound() {
            if (groundHitSounds != null)
                PlaySound(groundHitSounds.GetRandomHitSound());
        }
    }
}