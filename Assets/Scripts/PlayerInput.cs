using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _leftPaw;
    [SerializeField] private KeyCode _rightPaw;

    public bool GetLeftPawDown()
    {
        return Input.GetKeyDown(_leftPaw);
    }

    public bool GetRightPawDown()
    {
        return Input.GetKeyDown(_rightPaw);
    }
}
