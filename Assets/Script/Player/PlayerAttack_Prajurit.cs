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

    [SerializeField] private GameObject attackRagePrefab;
    public Transform spawnPoint; // Posisi spawn arrow
    public Transform attackTarget; // Target posisi serangan
    public bool IsAttacking { get; private set; } = false;
    public static event System.Action OnAttackDone;
    [SerializeField] private int _playerDamage;
    [SerializeField] private float arrowForce = 0f;
    private float originalArrowForce;
    private Coroutine _attackRoutine = null; //use to run attack with duration
    private GameObject _currentAttack;

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
        originalArrowForce = arrowForce;
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
        _currentAttack = attackPrefab;
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        RageMode.OnRageMode += RageModeRange;
        RageMode.OnRageMode += UpdateAttack;
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
        RageMode.OnRageMode -= RageModeRange;
        RageMode.OnRageMode -= UpdateAttack;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        if (_attackRoutine == null && !_playerMovement.IsDodging && Time.timeScale == 1)
        {
            Debug.Log("Attack");
            AudioManager.Instance.PlaySFX("Prajurit Att");
            _attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        IsAttacking = true;
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.25f);
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
        yield return new WaitForSeconds(_attackDuration);

        IsAttacking = false;
        _attackRoutine = null; // Set _attackRoutine kembali ke null agar serangan bisa dimulai kembali
        OnAttackDone?.Invoke();
    }


    // Method untuk menetapkan target posisi serangan
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    public void RageModeRange(bool isRageModeOn)
    {
        if (isRageModeOn)
        {
            arrowForce *= 2;
        }
        else
        {
            arrowForce = originalArrowForce;
        }
    }

    private void UpdateAttack(bool isRage)
    {
        _currentAttack = isRage ? attackRagePrefab : attackPrefab;
    }
}
