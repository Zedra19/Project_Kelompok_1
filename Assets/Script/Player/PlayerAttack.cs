using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    public bool IsAttacking { get; private set; } = false;
    public static event Action OnAttackDone;
    public Transform attackTrigger;
    [SerializeField] private GameObject attackVFXPrefab; 
    [SerializeField] private Animator _animator;
    [SerializeField] float _attackDuration;
    [SerializeField] private int _playerDamage;

    private PlayerMovement _playerMovement;
    private PlayerInput _playerInput;
    private Coroutine _attackRoutine = null; //use to run attack with duration
    private SFX _sfx;

    public int PlayerDamage
    {
        get
        {
            return _playerDamage;
        }
        set
        {
            _playerDamage = value;
        }
    }
    private void Update()
    {
        Debug.Log("PlayerDamage: " + _playerDamage);
    }

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Attack.performed += OnAttack;
        _sfx = GetComponent<SFX>();
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        //only attack if currently not attacking and not dodging
        if (_attackRoutine == null && !_playerMovement.IsDodging)
        {
            AudioManager.Instance.PlaySFX("Kesatria Att");
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
    }
// private IEnumerator AttackRoutine()
// {
//     _animator.SetTrigger("Attack");

//     // Instantiate attackVFX with the position and rotation of attackTrigger
//     Quaternion reversedRotation = attackTrigger.rotation * Quaternion.Euler(0, 180, 0); // Rotate by 180 degrees around the Y-axis
//     GameObject attackVFX = Instantiate(attackVFXPrefab, attackTrigger.position, reversedRotation);

//     IsAttacking = true;
//     yield return new WaitForSeconds(_attackDuration);
//     _attackRoutine = null;
//     IsAttacking = false;
//     Destroy(attackVFX);
//     OnAttackDone?.Invoke(); //invoke event to notify other script that attack is done, making enemy L hitable in another attack
// }

private IEnumerator AttackRoutine()
{
    _animator.SetTrigger("Attack");

    // Determine the position to instantiate attackVFX
    Vector3 offset = -attackTrigger.forward * 1.5f; // Move backward by 0.5 units
    Vector3 attackPosition = attackTrigger.position + offset;

    // Instantiate attackVFX with the adjusted position and rotation of attackTrigger
    Quaternion reversedRotation = attackTrigger.rotation * Quaternion.Euler(0, 180, 0); // Rotate by 180 degrees around the Y-axis
    GameObject attackVFX = Instantiate(attackVFXPrefab, attackPosition, reversedRotation);

    IsAttacking = true;
    yield return new WaitForSeconds(_attackDuration);
    _attackRoutine = null;
    IsAttacking = false;
    Destroy(attackVFX);
    OnAttackDone?.Invoke(); //invoke event to notify other script that attack is done, making enemy L hitable in another attack
}



}
