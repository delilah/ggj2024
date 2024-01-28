using System;
using UnityEditor;
using UnityEngine;

public class CakeLayer : MonoBehaviour
{
    private bool _hasLanded = false;
    [SerializeField] private int _cakeLayer;

    [SerializeField] AudioClip[] _fallingClips;
    [SerializeField] AudioClip[] _landedClips;
    [SerializeField] AudioClip[] _destroyedClips;

    [SerializeField] bool _clipIsGood;

    [SerializeField] private int _minimumSceneY = -5;

    private void Start()
    {
        SoundPlayer.Instance.PlayRandomSample(_fallingClips);
    }

    private void Update()
    {
        if (transform.position.y < _minimumSceneY)
        {
            Destroy(gameObject);
        }
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

        SoundPlayer.Instance.PlayRandomSample(_landedClips);

        if (_clipIsGood)
        {
            CatAudioManager.Instance.PlayGoodLayer();
        }
        else
        {
            CatAudioManager.Instance.PlayBadLayer();
        }
    }

    private void OnDestroy()
    {
        SoundPlayer.Instance.PlayRandomSample(_destroyedClips);
    }
}
