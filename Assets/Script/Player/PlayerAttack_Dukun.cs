using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack_Dukun : MonoBehaviour, IPlayerAttack
{
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackDuration;
    [SerializeField] private GameObject attackPointPrefab; // Objek attackPoint yang akan di-spawn
    [SerializeField] private GameObject attackAreaPrefab;  // Objek attackArea yang akan di-spawn
    [SerializeField] private GameObject attackVFXPrefab;  // Objek attackVFX yang akan di-spawn
    [SerializeField] private GameObject attackVFXRagePrefab;  // Objek attackVFXRage yang akan di-spawn
    public Transform attackTarget; // Target posisi serangan
    public float CastDuration = 2f;
    public float SpellDuration = 1f;
    public bool IsAttacking { get; private set; } = false;
    public static event System.Action OnAttackDone;
    [SerializeField] private int _playerDamage;
    private Coroutine _attackRoutine = null;
    private SFX _sfx;
    private GameObject _currentAttackVFX;


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
        GameObject pointerObject = GameObject.Find("Pointer");
        if (pointerObject != null)
        {
            attackTarget = pointerObject.transform;
        }
        else
        {
            Debug.LogWarning("Pointer object not found in the scene.");
        }
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = new PlayerInput();
        _playerInput.CharacterControls.Attack.performed += OnAttack;
        _sfx = GetComponent<SFX>();
        _currentAttackVFX = attackVFXPrefab;
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        RageMode.OnRageMode += UpdateAttackVFX;
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
        RageMode.OnRageMode -= UpdateAttackVFX;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //only attack if currently not attacking and not dodging
        if (_attackRoutine == null && !_playerMovement.IsDodging && Time.timeScale == 1)
        {
            // Spawn objek attackPoint di posisi attackTarget
            GameObject attackPoint = Instantiate(attackPointPrefab, attackTarget.position, attackTarget.rotation);

            // Mulai serangan
            AudioManager.Instance.PlaySFX("Meteor Dukun");
            _attackRoutine = StartCoroutine(AttackRoutine(attackPoint));
        }
    }

    private IEnumerator AttackRoutine(GameObject attackPoint)
    {
        GameObject attackVFX = Instantiate(_currentAttackVFX, attackPoint.transform.position, Quaternion.identity);
        // Animasi serangan
        _animator.SetTrigger("Attack");
        IsAttacking = true;

        // Tunggu sejenak sebelum mengubah attackPoint menjadi attackArea
        yield return new WaitForSeconds(CastDuration);

        // Spawn objek attackArea di posisi attackPoint
        GameObject attackArea = Instantiate(attackAreaPrefab, attackPoint.transform.position, attackPoint.transform.rotation);

        // Hancurkan attackPoint
        Destroy(attackPoint);

        // Tunggu 2 detik
        yield return new WaitForSeconds(SpellDuration);

        // Hancurkan attackArea
        Destroy(attackArea);
        Destroy(attackVFX);

        // Tunggu sampai animasi selesai
        yield return new WaitForSeconds(_attackDuration);

        // Selesaikan serangan
        _attackRoutine = null;
        IsAttacking = false;
        OnAttackDone?.Invoke();
    }

    // Method untuk menetapkan target posisi serangan
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    private void UpdateAttackVFX(bool isRage)
    {
        if (isRage)
        {
            _currentAttackVFX = attackVFXRagePrefab;
        }
        else
        {
            _currentAttackVFX = attackVFXPrefab;
        }
    }
}
