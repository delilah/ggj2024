using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMissedItem : MonoBehaviour
{

    private int _missedCounter = 0;
    const int NUMBER_OF_MISSES = 15;
    void OnTriggerEnter(Collider other)
    {
        _missedCounter++;

        if (_missedCounter % NUMBER_OF_MISSES == 0)
        {
           GameMessages.RequestBadLayer();
        }
    }
}
