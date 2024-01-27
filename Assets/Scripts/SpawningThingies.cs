using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningThingies : MonoBehaviour
{
    public GameObject[] layerPrefabs;
    //public GameObject[] YummyLayerPrefabs;
    //public GameObject[] YuckyLayerPrefabs;
    [SerializeField] private GameObject _spawningPoint;

    void Start()
    {
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

        GameObject spawnedLayer = Instantiate(layerPrefabs[layerType], _spawningPoint.transform.position, Quaternion.identity);
    }
    
}