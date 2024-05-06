using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    public bool IsAttacking { get; private set; } = false;
    public static event Action OnAttackDone;
    
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
