using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour, IService
{
    public bool JumpButtonDown => _jumpButtonDown;
    public bool MoveLeftButtonDown => _moveLeftButtonDown;
    public bool MoveRightButtonDown => _moveRightButtonDown;

    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _leftKey;
    [SerializeField] private KeyCode _rightKey;

    private bool _jumpButtonDown;
    private bool _moveLeftButtonDown;
    private bool _moveRightButtonDown;

    public void ResetJumpRequest()
    {
        _jumpButtonDown = false;
    }

    private void Update()
    {
        _jumpButtonDown = !_jumpButtonDown ? Input.GetKeyDown(_jumpKey) : true;
        _moveLeftButtonDown = Input.GetKeyDown(_leftKey);
        _moveRightButtonDown =  Input.GetKeyDown(_rightKey);
    }
}