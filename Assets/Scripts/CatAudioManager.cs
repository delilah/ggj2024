using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAudioManager : MonoBehaviour
{
    public static CatAudioManager Instance { get; private set; }

    [SerializeField] AudioClip[] _kneadingClips;
    [SerializeField] AudioClip[] _goodLayerClips;
    [SerializeField] AudioClip[] _badLayerClips;

    private void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// Play this when a cat paw moves (hit or miss doesn't matter)
    /// </summary>
    public void PlayKneading()
    {
        SoundPlayer.Instance.PlayRandomSample(_kneadingClips);
    }

    public void PlayGoodLayer()
    {
        SoundPlayer.Instance.PlayRandomSample(_goodLayerClips);
    }

    public void PlayBadLayer()
    {
        SoundPlayer.Instance.PlayRandomSample(_badLayerClips);
    }
}
