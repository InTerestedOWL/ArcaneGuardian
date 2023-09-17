using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase {
    RestPhase,
    NormalWave,
    ElfWave,
    HumanWave,
    GoblinWave,
    SkeletonWave,
    BossWave,
}

public class GlobalAudioSystem : MonoBehaviour {
    public List<AudioClip> restPhaseMusic;
    public List<AudioClip> normalWaveMusic;
    public List<AudioClip> elfWaveMusic;
    public List<AudioClip> humanWaveMusic;
    public List<AudioClip> goblinWaveMusic;
    public List<AudioClip> skeletonWaveMusic;
    public List<AudioClip> bossWaveMusic;
    [SerializeField]
    private AudioSource audioSourceMusic;
    [SerializeField]
    private AudioSource audioSourceVoices;
    [SerializeField]
    private AudioSource audioSourceUI;
    [SerializeField]
    private AudioClip uiHoverSound;
    [SerializeField]
    private AudioClip uiClickSound;
    [SerializeField]
    private AudioClip uiPopupOpenSound;
    [SerializeField]
    private AudioClip uiPopupCloseSound;
    [SerializeField]
    private AudioClip uiMessageSound;
    [SerializeField]
    private AudioClip uiDeclineSound;

    private void Awake() {
        audioSourceUI.ignoreListenerPause = true;
        if (audioSourceMusic != null) {
            audioSourceMusic.loop = true;
        }
    }

    public void PlayUIHoverSound() {
        if (uiHoverSound != null) {
            audioSourceUI.PlayOneShot(uiHoverSound);
        }
    }

    public void PlayUIClickSound() {
        if (uiClickSound != null) {
            audioSourceUI.PlayOneShot(uiClickSound);
        }
    }

    public void PlayUIPopupOpenSound() {
        if (uiPopupOpenSound != null) {
            audioSourceUI.PlayOneShot(uiPopupOpenSound);
        }
    }

    public void PlayUIPopupCloseSound() {
        if (uiPopupCloseSound != null) {
            audioSourceUI.PlayOneShot(uiPopupCloseSound);
        }
    }

    public void PlayUIMessageSound() {
        if (uiMessageSound != null) {
            audioSourceUI.PlayOneShot(uiMessageSound);
        }
    }

    public void PlayUIDeclineSound() {
        if (uiDeclineSound != null) {
            audioSourceUI.PlayOneShot(uiDeclineSound);
        }
    }

    private void Start() {
        PlayMusic(GamePhase.RestPhase);
    }

    public void PlayMusic(GamePhase phase) {
        List<AudioClip> clipsToPlay = GetClipsForPhase(phase);
        if (clipsToPlay != null && clipsToPlay.Count > 0 && audioSourceMusic != null) {
            GetRandomMusicClip(clipsToPlay);
        }
    }

    public void PlayVoice(AudioClip clip) {
        if (clip != null && audioSourceVoices != null) {
            audioSourceVoices.PlayOneShot(clip);
        }
    }

    public void PauseMusic() {
        if (audioSourceMusic != null) {
            audioSourceMusic.Pause();
        }
    }

    public void ResumeMusic() {
        if (audioSourceMusic != null) {
            audioSourceMusic.UnPause();
        }
    }

    private void GetRandomMusicClip(List<AudioClip> clips) {
        if (clips.Count > 0) {
            int randomIndex = Random.Range(0, clips.Count);
            audioSourceMusic.clip = clips[randomIndex];
            audioSourceMusic.Play();
        }
    }

    private List<AudioClip> GetClipsForPhase(GamePhase phase) {
        switch (phase) {
            case GamePhase.RestPhase:
                return restPhaseMusic;
            case GamePhase.NormalWave:
                return normalWaveMusic;
            case GamePhase.ElfWave:
                return elfWaveMusic;
            case GamePhase.HumanWave:
                return humanWaveMusic;
            case GamePhase.GoblinWave:
                return goblinWaveMusic;
            case GamePhase.SkeletonWave:
                return skeletonWaveMusic;
            case GamePhase.BossWave:
                return bossWaveMusic;
            default:
                return new List<AudioClip>();
        }
    }
}
