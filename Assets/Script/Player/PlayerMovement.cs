using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public bool IsDodging { get; private set; } = false;

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _visualObject; // Reference to the object representing the player visually
    [SerializeField] private Transform _pointer; // Reference to object to face

    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _maxSpeedMultiplier = 2;
    [SerializeField] private float _runMultiplier = 2f;
    [SerializeField] private float _dodgeForce = 8f;
    [SerializeField] private float _dodgeMaxForceMultiplier = 4f;
    [SerializeField] private float _dodgeDuration = 1f;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxRunSpeed;
    [SerializeField] private float _maxDodgeForce;

    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private Stamina _stamina;
    private PlayerAttack _playerAttack;
    private PlayerAttack_Dukun _playerAttackDukun;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _currentRunMovement;
    private Vector3 _lastPosition;

    private bool _isAllowedToDodge = true;
    private int _facingAnimatorState = 0;
    private bool _isRunPressed;
    private bool _isMovementPressed;
    private float _rotationFactorPerFrame = 15f;

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;

        _playerInput.CharacterControls.Run.started += OnRunInput;
        _playerInput.CharacterControls.Run.canceled += OnRunInput;

        _playerInput.CharacterControls.Dodge.performed += OnDodgeInput;
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Move.started -= OnMovementInput;
        _playerInput.CharacterControls.Move.canceled -= OnMovementInput;
        _playerInput.CharacterControls.Move.performed -= OnMovementInput;

        _playerInput.CharacterControls.Run.started -= OnRunInput;
        _playerInput.CharacterControls.Run.canceled -= OnRunInput;

        _playerInput.CharacterControls.Dodge.performed -= OnDodgeInput;
        _playerInput.CharacterControls.Disable();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _stamina = GetComponent<Stamina>();
        _playerInput = new PlayerInput();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerAttackDukun = GetComponent<PlayerAttack_Dukun>();
        _lastPosition = transform.position;



        _maxSpeed = _speed * _maxSpeedMultiplier;
        _maxRunSpeed = _maxSpeed * _runMultiplier;
        _maxDodgeForce = _dodgeForce * _dodgeMaxForceMultiplier;
    }


    private void OnDodgeInput(InputAction.CallbackContext context)
    {
        GameObject playerKsatria = GameObject.Find("Player-Ksatria");
        GameObject playerDukun = GameObject.Find("Player_Dukun");
        if (playerKsatria != null)
        {
            if ((!IsDodging || _isAllowedToDodge) && _stamina.CurrentStamina >= 1 && _playerAttack.IsAttacking == false)
            {
                StartCoroutine(Dodge());
            }
        }
        else if (playerDukun != null)
        {
            if ((!IsDodging || _isAllowedToDodge) && _stamina.CurrentStamina >= 1 && _playerAttackDukun.IsAttacking == false)
            {
                StartCoroutine(Dodge());
            }
        }

    }

    private IEnumerator Dodge()
    {
        _animator.SetTrigger("Dodge");
        Debug.Log("Dodge");
        _rigidbody.velocity = Vector3.zero; //make previous velocity to 0 so doesnt make dodge feel too fast/ too slow
        IsDodging = true;
        _animator.SetBool("Move", false);
        // Get the dodge direction based on visual object's rotation
        Vector3 dodgeDirection = _visualObject.forward * -1f; // Push back
        // Apply a strong force in the dodge direction
        _rigidbody.AddForce(dodgeDirection * _dodgeForce, ForceMode.VelocityChange);
        yield return new WaitForSeconds(_dodgeDuration);
        IsDodging = false;
    }


    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
        _currentMovementInput *= _speed;

        _currentMovement = transform.forward * _currentMovementInput.y + transform.right * _currentMovementInput.x;
        _currentRunMovement = _currentMovement * _runMultiplier;


        Debug.Log("Current Movement: " + _currentMovement);
        Debug.Log($"Velocity{_rigidbody.velocity}");
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
        if (IsDodging)
        {
            _rigidbody.maxLinearVelocity = _maxDodgeForce;
            return;
        }
        HandleMovement();
        SetMovementFacingDirection();
    }

    private void HandleMovementAnimation()
    {
        if (_rigidbody.velocity.x > 2 || _rigidbody.velocity.z > 2 || _rigidbody.velocity.x < -2 || _rigidbody.velocity.z < -2)
        {
            _animator.SetBool("Move", true);
        }
        else
        {
            _animator.SetBool("Move", false);
        }
    }

    private void HandleMovement()
    {
        Debug.Log("Handle Movement ");
        if (_isRunPressed)
        {
            _rigidbody.AddForce(_currentRunMovement, ForceMode.VelocityChange);
            _rigidbody.maxLinearVelocity = _maxRunSpeed;
            //so its not faster when moving in two axis
            if ((_currentMovement.x > 2 || _currentMovement.x < -2) && (_currentMovement.z > 2 || _currentMovement.z < -2))
            {
                _rigidbody.maxLinearVelocity = _maxRunSpeed / 2;
            }
        }
        else
        {
            _rigidbody.AddForce(_currentMovement, ForceMode.VelocityChange);
            _rigidbody.maxLinearVelocity = _maxSpeed;
            //so its not faster when moving in two axis
            if ((_currentMovement.x > 2 || _currentMovement.x < -2) && (_currentMovement.z > 2 || _currentMovement.z < -2))
            {
                _rigidbody.maxLinearVelocity = _maxSpeed / 2;
            }
        }
        HandleMovementAnimation();
        if ((_rigidbody.velocity.x < 1 && _rigidbody.velocity.x > -1) || (_rigidbody.velocity.z > -1 && _rigidbody.velocity.z < 1))
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
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

    private void SetMovementFacingDirection()
    {
        Vector3 targetPosition = _pointer.position;
        Vector3 objectPosition = transform.position;

        bool onTop = objectPosition.z > targetPosition.z;
        bool bottom = objectPosition.z < targetPosition.z;
        bool right = objectPosition.x > targetPosition.x;
        bool left = objectPosition.x < targetPosition.x;

        float distanceInZAxis = GetPositiveDistance(objectPosition.z, targetPosition.z);
        float distanceInXAxis = GetPositiveDistance(objectPosition.x, targetPosition.x);

        if (onTop && right)
        {
            Debug.Log("Masuk On Top Right");
            if (distanceInXAxis > distanceInZAxis)//TODO: it doesnt solve is it  clooser to z axis or x axis    
            {
                Debug.Log("Posisi Player di Atas Kanan Atas Pointer");
                // position of player is on Right of the pointer
                AnimateByDirection(3, 0, 2, 1);
            }
            if (distanceInXAxis < distanceInZAxis)
            {

                Debug.Log("Posisi Player di Atas Pointer");
                // position of player is on Top of the pointer
                AnimateByDirection(1, 2, 3, 0);
            }
        }

        if (onTop && left)
        {
            if (distanceInXAxis > distanceInZAxis)
            {
                Debug.Log("Posisi Player di Atas kiri Bawah Pointer");
                // position of player is on Left of the pointer
                AnimateByDirection(0, 3, 2, 1);
            }
            if (distanceInXAxis < distanceInZAxis)
            {
                // position of player is on Top of the pointer
                Debug.Log("Posisi Player di Atas kiri Atas Pointer");
                // position of player is on Top of the pointer

                AnimateByDirection(2, 1, 3, 0);
            }
        }

        if (bottom && right)
        {
            if (distanceInXAxis > distanceInZAxis)
            {

                // position of player is on Right of the pointer
                Debug.Log("Posisi Player di Bawah Kanan Atas Pointer");
                AnimateByDirection(3, 0, 2, 1);

            }
            if (distanceInXAxis < distanceInZAxis)
            {
                // position of player is on Bottom of the pointer
                Debug.Log("Posisi Player di Bawah Kanan Bawah Pointer");
                AnimateByDirection(2, 1, 0, 3);
            }
        }

        if (bottom && left)
        {
            if (distanceInXAxis > distanceInZAxis)
            {
                // position of player is on Left of the pointer
                Debug.Log("Posisi Player di Bawah Kiri Atas Pointer");
                AnimateByDirection(0, 3, 2, 1);
            }
            if (distanceInXAxis < distanceInZAxis)
            {
                // position of player is on Bottom of the pointer
                Debug.Log("Posisi Player di Bawah Kiri Bawah Pointer");
                AnimateByDirection(1, 2, 0, 3);
            }
        }


        _lastPosition = transform.position;
        /*  _lastFacingAnimationState = _facingAnimatorState;
          if (_lastFacingAnimationState == _facingAnimatorState)
          {
              return;
          }
          */

        // Set the animator variable:
        _animator.SetInteger("MovementDirection", _facingAnimatorState);

    }

    private float GetPositiveDistance(float value1, float value2)
    {
        // Calculate the absolute difference between the values
        float difference = Mathf.Abs(value1 - value2);
        // Ensure the result is always positive
        return difference;
    }

    private void AnimateByDirection(int animationCode1, int animationCode2, int animationCode3, int animationCode4)
    {
        if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
        {
            _facingAnimatorState = animationCode1;
        }
        if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
        {
            _facingAnimatorState = animationCode2;
        }
        if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
        {
            _facingAnimatorState = animationCode3;
        }
        if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
        {
            _facingAnimatorState = animationCode4;
        }
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        _maxSpeed = _speed * _maxSpeedMultiplier;
    }

    public void SetRunMultiplier(float runMultiplier)
    {
        _runMultiplier = runMultiplier;
        _maxRunSpeed = _runMultiplier * _maxSpeedMultiplier;
    }

    public void SetDodge(float dodgeForce)
    {
        _dodgeForce = dodgeForce;
        _maxDodgeForce = _dodgeForce * _dodgeMaxForceMultiplier;
    }
}