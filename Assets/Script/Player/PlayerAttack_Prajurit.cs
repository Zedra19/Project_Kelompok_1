using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack_Prajurit : MonoBehaviour, IPlayerAttack
{
    private PlayerInput _playerInput;
    private PlayerMovement _playerMovement;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackDuration;
    [SerializeField] private GameObject attackPrefab;  // Objek attackArea yang akan di-spawn
    public Transform spawnPoint;
    public Transform attackTarget; // Target posisi serangan
    public float attackDuration = 2f;
    // public float SpellDuration = 1f;
    public bool IsAttacking { get; private set; } = false;
    public static event System.Action OnAttackDone;
    [SerializeField] private int _playerDamage;
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
        //only attack if currently not attacking and not dodging
        if (_attackRoutine == null && !_playerMovement.IsDodging)
        {
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
        // else if (_attackRoutine != null)
        // {
        //     StopCoroutine(_attackRoutine);
        //     _attackRoutine = null;
        // }
    }

    private IEnumerator AttackRoutine()
    {
        IsAttacking = true;

        // Spawn panah pada spawn point
        GameObject arrow = Instantiate(attackPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

        // Menghitung arah dari spawn point ke target
        Vector3 direction = (attackTarget.position - spawnPoint.position).normalized;

        // Memberikan gaya melambung ke panah
        arrowRigidbody.AddForce(direction * 10f, ForceMode.Impulse);

        // // Menunggu hingga panah mencapai target atau jarak yang sangat dekat dengan target
        // yield return new WaitUntil(() => Vector3.Distance(arrow.transform.position, attackTarget.position) < 0.1f);

        // // Menghancurkan panah
        // Destroy(arrow);

        // Menunggu sejenak sebelum serangan selesai
        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
        _attackRoutine = null; // Set _attackRoutine kembali ke null agar serangan bisa dimulai kembali
        OnAttackDone?.Invoke();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag == "Spear" && collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

    // Method untuk menetapkan target posisi serangan
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }
}
