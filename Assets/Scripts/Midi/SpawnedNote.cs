using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedNote : MonoBehaviour
{
    [SerializeField] private GameObject _tooLate;

    public TimedNote TimedNote { get; set; }

    private void OnEnable()
    {
        _tooLate.SetActive(false);
    }

    public void SetInRange(bool isNoteInRangeOfHit)
    {
        transform.localScale = isNoteInRangeOfHit ? Vector3.one * 1.5f : Vector3.one;
    }

    public void SetTooLate()
    {
        _tooLate.SetActive(true);
    }
}