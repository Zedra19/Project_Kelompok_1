using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;

    [SerializeField] private Transform _visualObject; // Reference to the object representing the player visually
    [SerializeField] private Transform _pointer; // Reference to object to face

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _runMultiplier = 2f;
    [SerializeField] private float _maxRunSpeed = 8f;
    [SerializeField] private float _dodgeForce = 7f;
    [SerializeField] private float _maxDodgeSpeed = 20f;
    [SerializeField] private float _dodgeDuration = 1f;
    private bool _isDodging = false;
    private bool _isAllowedToDodge = true;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _currentRunMovement;

    private bool _isRunPressed;
    private bool _isMovementPressed;
    private float _rotationFactorPerFrame = 15f;

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _playerInput = new PlayerInput();

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;

        _playerInput.CharacterControls.Run.started += OnRunInput;
        _playerInput.CharacterControls.Run.canceled += OnRunInput;

        _playerInput.CharacterControls.Dodge.performed += OnDodgeInput;
    }

    private void OnDodgeInput(InputAction.CallbackContext context)
    {
        if (!_isDodging || _isAllowedToDodge)
        {
            StartCoroutine(Dodge());
        }
    }

    private IEnumerator Dodge()
    {
        _isDodging = true;
        // Get the dodge direction based on visual object's rotation
        Vector3 dodgeDirection = _visualObject.forward * -1f; // Push back
        // Apply a strong force in the dodge direction
        _rigidbody.AddForce(dodgeDirection * _dodgeForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(_dodgeDuration);
        _isDodging = false;
    }


    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
        _currentMovementInput *= _speed;

        _currentMovement = transform.forward * _currentMovementInput.y + transform.right * _currentMovementInput.x;
        _currentRunMovement = _currentMovement * _runMultiplier;
    }

    private void OnRunInput(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (_pointer != null)
        {
            HandleRotation(_pointer.position);
        }
    }

    private void FixedUpdate() // Use FixedUpdate for physics updates
    {
        if (_isDodging)
        {
            _rigidbody.maxLinearVelocity = _maxDodgeSpeed;
            return;
        }
        if (_isRunPressed)
        {
            _rigidbody.AddForce(_currentRunMovement, ForceMode.VelocityChange);
            _rigidbody.maxLinearVelocity = _maxRunSpeed;
        }
        else
        {

            _rigidbody.AddForce(_currentMovement, ForceMode.VelocityChange);
            _rigidbody.maxLinearVelocity = _maxSpeed;
        }
    }

    private void HandleRotation(Vector3 targetPosition)
    {
        Vector3 positionToLookAt = targetPosition - transform.position;
        positionToLookAt.y = 0f; // Ignore vertical position

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

        // Rotate the visual object, not the Rigidbody
        _visualObject.rotation = Quaternion.Slerp(_visualObject.rotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
    }
}