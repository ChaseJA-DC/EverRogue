using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;
    private PlayerInput _playerInput;
    private InputAction _moveAction;


    // Start is called before the first frame update
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
    }

    // Update is called once per frame
    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();
    }
}
