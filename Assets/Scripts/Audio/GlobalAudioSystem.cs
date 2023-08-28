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
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null) {
            audioSource.loop = true;
        }
    }

    private void Start() {
        PlayMusic(GamePhase.RestPhase);
    }

    public void PlayMusic(GamePhase phase) {
        List<AudioClip> clipsToPlay = GetClipsForPhase(phase);
        if (clipsToPlay != null && clipsToPlay.Count > 0 && audioSource != null) {
            int randomIndex = Random.Range(0, clipsToPlay.Count);
            audioSource.clip = clipsToPlay[randomIndex];
            audioSource.Play();
        }
    }

    private void GetRandomMusicClip(List<AudioClip> clips) {
        if (clips.Count > 0) {
            int randomIndex = Random.Range(0, clips.Count);
            audioSource.clip = clips[randomIndex];
            audioSource.Play();
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
