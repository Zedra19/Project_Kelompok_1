using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack_Prajurit : MonoBehaviour, IPlayerAttack
{
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackDuration;
    [SerializeField] private GameObject attackPrefab;  // Objek arrow yang akan di-spawn
    public Transform spawnPoint; // Posisi spawn arrow
    public Transform attackTarget; // Target posisi serangan
    public float attackDuration = 0f;
    public bool IsAttacking { get; private set; } = false;
    public static event System.Action OnAttackDone;
    [SerializeField] private int _playerDamage;
    [SerializeField] private float arrowForce = 10f;
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (_attackRoutine == null && !_playerMovement.IsDodging)
        {
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        IsAttacking = true;
        GameObject arrow = Instantiate(attackPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();
        Vector3 attackTargetWithoutYAxis = new Vector3(attackTarget.position.x, spawnPoint.position.y, attackTarget.position.z);
        Vector3 direction = (attackTargetWithoutYAxis - spawnPoint.position).normalized;
        // Menentukan rotasi untuk panah agar sumbu z menghadap ke arah target
        Quaternion rotation = Quaternion.LookRotation(direction);
        // Memutar panah sesuai dengan rotasi yang dihitung
        arrow.transform.rotation = rotation;
        // Memberikan gaya melambung ke panah
        arrowRigidbody.AddForce(arrow.transform.forward * arrowForce, ForceMode.Impulse);
        yield return new WaitForSeconds(attackDuration);

        IsAttacking = false;
        _attackRoutine = null; // Set _attackRoutine kembali ke null agar serangan bisa dimulai kembali
        OnAttackDone?.Invoke();
    }


    // Method untuk menetapkan target posisi serangan
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }
}
