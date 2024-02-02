using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnedNote : MonoBehaviour
{
    [SerializeField] private TMP_Text _inputHint;
    //[SerializeField] private GameObject _tooLate;

    public TimedNote TimedNote { get; set; }

    //private void OnEnable()
    //{
    //    _tooLate.SetActive(false);
    //}

    public void SetInRange(bool isNoteInRangeOfHit, string inputHint)
    {
        transform.localScale = isNoteInRangeOfHit ? Vector3.one * 1.5f : Vector3.one;
        _inputHint.gameObject.SetActive(isNoteInRangeOfHit);
        _inputHint.SetText(inputHint);
    }

    public void SetTooLate()
    {
        //_tooLate.SetActive(true);
    }
}