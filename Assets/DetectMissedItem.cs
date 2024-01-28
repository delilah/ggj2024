using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMissedItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // It's a miss, send a bad layer yo 
        //GameMessages.RequestBadLayer();
    }
}
