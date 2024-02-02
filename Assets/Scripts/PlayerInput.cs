using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _leftPaw;
    [SerializeField] private KeyCode _rightPaw;
    [SerializeField] private float _tapGrace = 0.2f;

    private float _leftPawTapTime;
    private float _rightPawTapTime;

    public bool GetLeftPawDown()
    {
        var tap = Time.time - _leftPawTapTime <= _tapGrace;
        _leftPawTapTime = 0f;
        return Input.GetKeyDown(_leftPaw) || tap;
    }

    public bool GetRightPawDown()
    {
        var tap = Time.time - _rightPawTapTime <= _tapGrace;
        _rightPawTapTime = 0f;
        return Input.GetKeyDown(_rightPaw) || tap;
    }

    public void ForceLeftPaw()
    {
        _leftPawTapTime = Time.time;
    }

    public void ForceRightPaw()
    {
        _rightPawTapTime = Time.time;
    }
}
