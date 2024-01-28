using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance { get; private set; }

    private Dictionary<string, AudioSource> _audioSources;

    private void Start()
    {
        _audioSources = GetComponentsInChildren<AudioSource>().ToDictionary(x => x.name, x => x);

        Instance = this;
    }

    public void PlayRandomSample(AudioClip[] audioClips)
    {
        PlayRandomClip(audioClips, "Samples");
    }

    private void PlayRandomClip(AudioClip[] audioClips, string type)
    {
        if (!_audioSources.ContainsKey(type))
        {
            throw new InvalidOperationException($"AudioSource {type} does not exist.");
        }

        if (_audioSources[type] == null)
        {
            return;
        }

        _audioSources[type].PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length)]);
    }
}
