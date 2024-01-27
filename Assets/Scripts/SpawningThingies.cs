using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningThingies : MonoBehaviour
{
    [SerializeField] private float _yOffsetPerLayer = 0.5f;

    public GameObject[] layerPrefabs;
    //public GameObject[] YummyLayerPrefabs;
    //public GameObject[] YuckyLayerPrefabs;
    [SerializeField] private Transform _spawningPoint;

    void Start()
    {
        var camera = Camera.main;
        Vector3[] frustumCorners = new Vector3[4];
        camera.CalculateFrustumCorners(
            new Rect(0, 0, 1, 1), 
            camera.transform.InverseTransformPoint(_spawningPoint.position).z, 
            Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        var worldSpaceCorner = camera.transform.TransformVector(frustumCorners[1]);


        var newPos = _spawningPoint.position;
        newPos.y = worldSpaceCorner.y;
        _spawningPoint.position = newPos;

        // every 2 seconds for now, will be based on action later
        InvokeRepeating("SpawnRandomLayer", 0f, 2f);
    }

    public void SpawnRandomLayer()
    {
        // random for now
        int layerType = Random.Range(0, layerPrefabs.Length);
        SpawnLayer(layerType);
    }

    public void SpawnLayer(int layerType)
    {
        if (layerType < 0 || layerType >= layerPrefabs.Length)
        {
            Debug.LogError("Invalid layer type!");
            return;
        }

        GameObject spawnedLayer = Instantiate(layerPrefabs[layerType], _spawningPoint.position, Quaternion.identity);
        var newPos = _spawningPoint.position;
        newPos += Vector3.up * _yOffsetPerLayer;
        _spawningPoint.position = newPos;
    }
}