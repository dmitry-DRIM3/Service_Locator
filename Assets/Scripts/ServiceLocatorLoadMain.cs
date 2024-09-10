using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorLoadMain : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private InputController _inputController;

    private void Awake()
    {
        ServiceLocator.RegisterService<PlayerMovementController>(_playerMovementController);
        ServiceLocator.RegisterService<InputController>(_inputController);     
    }
}
