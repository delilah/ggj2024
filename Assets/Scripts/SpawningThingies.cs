using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningThingies : MonoBehaviour
{
    [SerializeField] private float _yOffsetPerLayer = 0.5f;
    [SerializeField] private float _yOffsetCamera = 1.5f;
    [SerializeField] private Vector2 _xVariance = new Vector2(0.1f, 0.2f);

    [SerializeField] private CakeLayer[] _yummyLayerPrefabs;
    [SerializeField] private CakeLayer[] _yuckyLayerPrefabs;
    [SerializeField] private Transform _spawningPoint;
    [SerializeField] private Transform _highestCake;

    Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        UpdateSpawnPoint();
    }

    [ContextMenu("UpdatePoint")]
    public void UpdateSpawnPoint()
    {
        Vector3[] frustumCorners = new Vector3[4];
        _camera.CalculateFrustumCorners(
            new Rect(0, 0, 1, 1), 
            _camera.transform.InverseTransformPoint(_spawningPoint.position).z, 
            Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        var worldSpaceCorner = _camera.transform.TransformVector(frustumCorners[1]);

        var newPos = _spawningPoint.position;
        newPos.y = worldSpaceCorner.y;
        _spawningPoint.position = newPos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) SpawnRandomGoodLayer();
        if (Input.GetKeyDown(KeyCode.B)) SpawnRandomBadLayer();
    }

    private void LateUpdate()
    {
        var newPos = _spawningPoint.position;
        //newPos += Vector3.up * _yOffsetPerLayer;
        newPos.y = _camera.transform.position.y + _yOffsetCamera;
        _spawningPoint.position = newPos;
    }

    public void SpawnRandomGoodLayer()
    {
        SpawnLayer(RandomFromArray(_yummyLayerPrefabs), 0f);
        CakeRating.GoodLayers++;
    }

    public void SpawnRandomBadLayer()
    {
        SpawnLayer(RandomFromArray(_yuckyLayerPrefabs), 1f);
        CakeRating.BadLayers++;
    }

    public void SpawnLayer(CakeLayer prefab, float leaningMultiplier)
    {
        var side = Random.value > 0.5f ? 1f : -1f;
        var offset = Random.Range(_xVariance.x, _xVariance.y) * side * leaningMultiplier;

        var y = Random.Range(0f, 360f);
        GameObject spawnedLayer = Instantiate(prefab.gameObject, _spawningPoint.position + Vector3.right * offset, Quaternion.Euler(0f, y, 0f));
        
        
    }

    CakeLayer RandomFromArray(CakeLayer[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}