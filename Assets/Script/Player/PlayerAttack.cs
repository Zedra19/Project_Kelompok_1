using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttack : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Coroutine _attackRoutine = null; //use to run attack with duration
    [SerializeField] private Animator _animator;
    [SerializeField] float _attackDuration;
    public bool isAttacking = false;

    private void Awake()
    {
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
        //only attack if currently not attacking
        if (_attackRoutine == null)
        {
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        _animator.SetTrigger("Attack");
        isAttacking = true;
        yield return new WaitForSeconds(_attackDuration);
        _attackRoutine = null;
        if(_attackRoutine == null)
        {
            isAttacking = false;
        }
    }
}
