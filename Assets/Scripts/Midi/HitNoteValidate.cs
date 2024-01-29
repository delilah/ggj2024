using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNoteValidate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<SpawnedNote>(out var spawnedNote))
        {
            //Debug.Log($"Hit: {spawnedNote.TimedNote.Note}:{spawnedNote.TimedNote.TimeInSeconds} at {Time.realtimeSinceStartup}");
        }
    }
}
