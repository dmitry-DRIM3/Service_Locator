using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour, IService
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [SerializeField] private float _speedHorizontal;
    [SerializeField] private float _stepSizeHorizontal;

    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;
    
    private Rigidbody _rigidbody;
    private bool _isGrounded;
    private InputController _inputController;

    private Vector3 _positionHorizontal;

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _inputController = ServiceLocator.GetService<InputController>();            
    }

    private void Update()
    {
        MoveHorizontally();
    }

    private void FixedUpdate()
    {
        MoveVertically();
        Jump();
    }

    private void MoveHorizontally()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Lerp(position.x, _positionHorizontal.x, _speedHorizontal * Time.deltaTime);
        
        transform.position = position;

        float step;
        if (_inputController.MoveRightButtonDown)
        {
            step = _stepSizeHorizontal;
        }
        else if (_inputController.MoveLeftButtonDown)
        {
            step = -_stepSizeHorizontal;
        }
        else
        {
            return;
        }
       
        _positionHorizontal.x += step;
        _positionHorizontal.x = Mathf.Clamp(_positionHorizontal.x, _minX, _maxX);       
    }

    private void MoveVertically()
    {
        Vector3 velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _speed);
        _rigidbody.velocity = velocity;
    }

    private void Jump()
    {     
        if (_inputController.JumpButtonDown && _isGrounded)
        {
            _inputController.ResetJumpRequest();
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }   
    }

    private void OnCollisionStay(Collision collision)
    {
        if(Vector3.Dot(transform.position,collision.contacts[0].normal) > 0)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }
}
