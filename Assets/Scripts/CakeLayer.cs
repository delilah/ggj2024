using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CakeLayer : MonoBehaviour
{
    private bool _hasLanded = false;
    private float _layerHeight = 0;

    [SerializeField] private int _cakeLayer;

    [SerializeField] AudioClip[] _fallingClips;
    [SerializeField] AudioClip[] _landedClips;
    [SerializeField] AudioClip[] _destroyedClips;

    [SerializeField] bool _clipIsGood;

    [SerializeField] private int _minimumSceneY = -5;

    [SerializeField] private GameObject[] _collisionFxs;

    private void Start()
    {
        _layerHeight = gameObject.GetComponent<BoxCollider>().bounds.extents.y;

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

        RenderFx();
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
            return;

        SoundPlayer.Instance.PlayRandomSample(_destroyedClips);

        RenderFx();
    }

    private void RenderFx()
    {
        var position = gameObject.transform.position;
        position.y -= _layerHeight / 2;
        foreach (var collissionFx in _collisionFxs)
        {
            var collissionFxInstance = Instantiate(collissionFx, position, Quaternion.identity);
            Destroy(collissionFxInstance, 2.5f);
        }
    }
}
