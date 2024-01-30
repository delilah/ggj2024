using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance { get; private set; }

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _soundtrackSource;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if (_soundtrackSource.time >= _soundtrackSource.clip.length) GameMessages.NotifyGameOver();
    }

    public void PlayRandomSample(AudioClip[] audioClips)
    {
        _sfxSource.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length)]);
    }
}
