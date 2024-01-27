using System;
using UnityEngine;

public class CakeLayer : MonoBehaviour
{
    private bool _hasLanded = false;
    [SerializeField] private int _cakeLayer;

    [SerializeField] AudioClip[] _fallingClips;
    [SerializeField] AudioClip[] _landedClips;

    [SerializeField] AudioSource _fallingSource;
    [SerializeField] AudioSource _landedSource;

    private void Start()
    {
        PlayRandomClip(_fallingClips, _fallingSource);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasLanded)
        {
            return;
        }
        else
        {
            gameObject.layer = _cakeLayer;
        }
        _hasLanded = true;

        Debug.Log("Collision started");
        PlayRandomClip(_landedClips, _landedSource);
        Debug.Log("Collision ended");
    }

    private void PlayRandomClip(AudioClip[] audioClips, AudioSource audioSource)
    {
        if ( audioClips.Length == 0)
        {
            throw new InvalidOperationException("No AudioClips available on layer.");
        }

        if(audioSource == null)
        {
            throw new InvalidOperationException("No AudioSource defined.");
        }

        audioSource.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length - 1)]);
    }
}
