using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour, IInputService
{
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _leftKey;
    [SerializeField] private KeyCode _rightKey;

    public bool GetJumpButtonDown()
    {
        return Input.GetKeyDown(_jumpKey);
    }

    public bool GetMoveLeftButtonDown()
    {
        return Input.GetKeyDown(_leftKey);
    }

    public bool GetMoveRightButtonDown()
    {
        return Input.GetKeyDown(_rightKey);
    }
}
