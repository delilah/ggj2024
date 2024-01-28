using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private LoadSceneButton _loader;

    private void OnEnable()
    {
        GameMessages.OnGameOver += LoadResults;
    }

    private void OnDisable()
    {
        GameMessages.OnGameOver -= LoadResults;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) GameMessages.NotifyGameOver();
    }
#endif

    private void LoadResults()
    {
        _loader.Load();
    }
}
