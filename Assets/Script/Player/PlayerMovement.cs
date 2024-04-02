using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;
    private Stamina _stamina;
    private PlayerAttack _playerAttack;

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _visualObject; // Reference to the object representing the player visually
    [SerializeField] private Transform _pointer; // Reference to object to face

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _runMultiplier = 2f;
    [SerializeField] private float _maxRunSpeed = 8f;
    [SerializeField] private float _dodgeForce = 7f;
    [SerializeField] private float _maxDodgeSpeed = 20f;
    [SerializeField] private float _dodgeDuration = 1f;
    public bool IsDodging { get; private set; } = false;
    private bool _isAllowedToDodge = true;
    private int _facingAnimatorState = 0;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _currentRunMovement;
    private Vector3 _lastPosition;
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
        _stamina = GetComponent<Stamina>();
        _playerInput = new PlayerInput();
        _playerAttack = GetComponent<PlayerAttack>();
        _lastPosition = transform.position;

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;

        _playerInput.CharacterControls.Run.started += OnRunInput;
        _playerInput.CharacterControls.Run.canceled += OnRunInput;

        _playerInput.CharacterControls.Dodge.performed += OnDodgeInput;
    }

    private void OnDodgeInput(InputAction.CallbackContext context)
    {
        if ((!IsDodging || _isAllowedToDodge) && _stamina.CurrentStamina >= 1 && _playerAttack.IsAttacking == false)
        {
            StartCoroutine(Dodge());
        }
    }

    private IEnumerator Dodge()
    {
        _animator.SetTrigger("Dodge");
        IsDodging = true;
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
            _rigidbody.maxLinearVelocity = _maxDodgeSpeed;
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
                /*
                if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Mundur CODE 1");
                }
                if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Maju CODE 2");
                }
                if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Kanan CODE 3");
                }
                if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Kiri CODE 4");
                }
                */
                AnimateByDirection(3, 0, 2, 1);

            }
            if (distanceInXAxis < distanceInZAxis)
            {
                /*
                Debug.Log("Posisi Player di Atas Pointer");
                // position of player is on Top of the pointer
                if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Maju CODE 5");
                }
                if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Mundur CODE 6");
                }
                if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Kiri CODE 7");
                }
                if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Kanan CODE 8");
                }
                */
                AnimateByDirection(1, 2, 3, 0);
            }
        }

        if (onTop && left)
        {
            if (distanceInXAxis > distanceInZAxis)
            {
                Debug.Log("Posisi Player di Atas kiri Bawah Pointer");
                // position of player is on Left of the pointer

                /*
                if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Kiri CODE 9");
                }
                if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Kanan CODE 10");
                }
                if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Maju CODE 11");
                }
                if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Mundur CODE 12");
                }
                */
                AnimateByDirection(0, 3, 2, 1);
            }
            if (distanceInXAxis < distanceInZAxis)
            {
                // position of player is on Top of the pointer
                Debug.Log("Posisi Player di Atas kiri Atas Pointer");
                // position of player is on Top of the pointer

                /*
                if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Kiri CODE 13");
                }
                if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Kanan CODE 14");
                }
                if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Mundur CODE 15");
                }
                if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Maju CODE 16");
                }
                */
                AnimateByDirection(2, 1, 3, 0);
            }
        }

        if (bottom && right)
        {
            if (distanceInXAxis > distanceInZAxis)
            {

                // position of player is on Right of the pointer
                Debug.Log("Posisi Player di Bawah Kanan Atas Pointer");
                /*
                                if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
                                {
                                    Debug.Log("Jalan Kiri CODE 17");
                                }
                                if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
                                {
                                    Debug.Log("Jalan Kanan CODE 18");
                                }
                                if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
                                {
                                    Debug.Log("Jalan Mundur CODE 19");
                                }
                                if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
                                {
                                    Debug.Log("Jalan Maju CODE 20");
                                }
                                */
                AnimateByDirection(3, 0, 2, 1);

            }
            if (distanceInXAxis < distanceInZAxis)
            {
                // position of player is on Bottom of the pointer
                Debug.Log("Posisi Player di Bawah Kanan Bawah Pointer");

                /*
                            if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
                            {
                                Debug.Log("Jalan kanan CODE 21");
                            }
                            if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
                            {
                                Debug.Log("Jalan kiri CODE 22");
                            }
                            if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
                            {
                                Debug.Log("Jalan Mundur CODE 23");
                            }
                            if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
                            {
                                Debug.Log("Jalan Maju CODE 24");
                            }
                            */

                AnimateByDirection(2, 1, 0, 3);
            }
        }

        if (bottom && left)
        {
            if (distanceInXAxis > distanceInZAxis)
            {
                // position of player is on Left of the pointer
                Debug.Log("Posisi Player di Bawah Kiri Atas Pointer");
                /*
                if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Kiri CODE 25");
                }
                if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan Kanan CODE 26");
                }
                if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Maju CODE 27");
                }
                if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan Mundur CODE 28");
                }
                */
                AnimateByDirection(0, 3, 2, 1);
            }
            if (distanceInXAxis < distanceInZAxis)
            {
                // position of player is on Bottom of the pointer
                Debug.Log("Posisi Player di Bawah Kiri Bawah Pointer");
                /*
                if (transform.position.z < _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan mundur CODE 29");
                }
                if (transform.position.z > _lastPosition.z && transform.position.z != _lastPosition.z)
                {
                    Debug.Log("Jalan maju CODE 30");
                }
                if (transform.position.x > _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan kanan CODE 31");
                }
                if (transform.position.x < _lastPosition.x && transform.position.x != _lastPosition.x)
                {
                    Debug.Log("Jalan kiri CODE 32");
                }
                */
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
}