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

    private SoundPlayer _soundPlayer;

    [SerializeField] private int _minimumSceneY = -5;

    private void Start()
    {
        var soundPlayerGameObject = GameObject.Find("SoundPlayer").gameObject;

        _soundPlayer = soundPlayerGameObject.GetComponent<SoundPlayer>();

        _soundPlayer.PlayRandomSample(_fallingClips);
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

        _soundPlayer.PlayRandomSample(_landedClips);
    }

    private void OnDestroy()
    {
        _soundPlayer.PlayRandomSample(_destroyedClips);
    }
}
