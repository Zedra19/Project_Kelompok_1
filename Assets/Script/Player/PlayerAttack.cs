using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class PlayerAttack : MonoBehaviour
{
    public bool IsAttacking { get; private set; } = false;
    [SerializeField] private Animator _animator;
    [SerializeField] float _attackDuration;
    private PlayerInput _playerInput;
    private Coroutine _attackRoutine = null; //use to run attack with duration
    private PlayerMovement _playerMovement;

    public static event Action<bool> OnAttackDamaging;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Attack.performed += OnAttack;
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
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        _animator.SetTrigger("Attack");
        IsAttacking = true;
        OnAttackDamaging?.Invoke(true);
        yield return new WaitForSeconds(_attackDuration);
        _attackRoutine = null;
        OnAttackDamaging?.Invoke(false);
        IsAttacking = false;
    }
}
