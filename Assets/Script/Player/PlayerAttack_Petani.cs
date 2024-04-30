using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttack_Petani : MonoBehaviour, IPlayerAttack
{
    public bool IsAttacking { get; private set; } = false;
    public static event Action OnAttackDone;
    private PlayerMovement _playerMovement;
    [SerializeField] private Animator _animator;
    [SerializeField] float _attackDuration;
    [SerializeField] private int _playerDamage;
    private PlayerInput _playerInput;
    private Coroutine _attackRoutine = null; //use to run attack with duration

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
        yield return new WaitForSeconds(_attackDuration);
        _attackRoutine = null;
        IsAttacking = false;
        OnAttackDone?.Invoke(); //invoke event to notify other script that attack is done, making enemy L hitable in another attack
    }
}
