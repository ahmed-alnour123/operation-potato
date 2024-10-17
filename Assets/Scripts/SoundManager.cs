using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;

    public AudioSource matchSource;
    public AudioSource mismatchSource;
    public AudioSource cardFlipSource;
    public AudioSource endgameSource;

    Dictionary<SoundEffect, AudioSource> soundsMap;

    void Awake() {
        Instance = this;

        soundsMap = new(){
          {SoundEffect.Match, matchSource},
          {SoundEffect.Mismatch, mismatchSource},
          {SoundEffect.CardFlip, cardFlipSource},
          {SoundEffect.Endgame, endgameSource},
        };
    }

    public void PlaySound(SoundEffect sound) {
        AudioSource source = soundsMap[sound];
        source.Play();
    }
}

public enum SoundEffect {
    Match,
    Mismatch,
    CardFlip,
    Endgame,
}
