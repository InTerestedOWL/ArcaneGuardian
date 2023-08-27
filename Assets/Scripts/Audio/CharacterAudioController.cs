using System.Collections;
using System.Collections.Generic;
using AG.Audio.Sounds;
using UnityEngine;

public class CharacterAudioController : MonoBehaviour {
    private AudioSource audioSource;
    [SerializeField]
    private FootstepSounds footstepSounds;
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
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Start() {
        
    }

    void Update() {
        
    }

    private void PlaySound(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayRandomFootstepSound(FootstepType type) {
        if (type == FootstepType.Running && (animator.GetFloat("walkSpeed") >= runningThreshold)) {
            PlaySound(footstepSounds.GetRandomRunningFootstepSound());
        } else if (type == FootstepType.Walking && (animator.GetFloat("walkSpeed") < runningThreshold)) {
            PlaySound(footstepSounds.GetRandomWalkingFootstepSound());
        }
    }
}
