using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public class PatternPatih : MonoBehaviour
{
    public static event Action OnPatihDestroyed;
    private Animator _animator;

    public int damage, HP, MaxHP;
    public float moveSpeed = 5f;
    public float attackRange = 7f;
    public float triggerRange = 4f;
    public float CastDuration;
    public float MaxCastDuration = 3f;
    public float StunDuration = 3f;

    public bool Charging = false;
    public bool CanMove = true;
    public bool IsStunned = false;
    public bool IsRage = false;
    public bool RageAnim = false;

    public Slider BossHealthSlider;
    public GameObject VFXAtt;
    public GameObject hitboxPrefab;
    public GameObject rageModeVFX;
    public GameObject ChargeVFX;
    public Transform rightHandTransform; // Tambahkan variabel ini

    private GameObject rageModeInstance;
    private GameObject chargeVFXInstance; // Tambahkan variabel ini

    private string playerTag = "Player";
    private Transform player;
    private NavMeshAgent _navAgent;
    private Health _playerHealth;
    private GameObject hitboxInstance;

    public bool IsGettingHitInThisHit = false;

    private void OnEnable()
    {
        PlayerAttack.OnAttackDone += AllowAttack;
    }

    private void OnDisable()
    {
        PlayerAttack.OnAttackDone -= AllowAttack;
    }

    private void AllowAttack()
    {
        IsGettingHitInThisHit = false;
    }

    void Start()
    {
        HP = MaxHP;
        CastDuration = MaxCastDuration;
        _animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        _playerHealth = GetComponent<Health>();
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.speed = moveSpeed;
    }

    void Update()
    {
        SetHP();
        CekBool();
    }

    public void CekBool()
    {
        if (!RageAnim && IsRage)
        {
            AudioManager.Instance.PlaySFX("RageSound");
            _animator.SetTrigger("Rage");
            RageAnim = true;

            GameObject Patih = GameObject.FindWithTag("Patih");
            rageModeInstance = Instantiate(rageModeVFX, Patih.transform.position, Quaternion.identity);
            rageModeInstance.transform.SetParent(transform);
        }
        if (CanMove && !IsStunned)
        {
            _animator.SetBool("isMoving", true);
        }

        if (!CanMove && Charging)
        {
            _animator.SetBool("isMoving", false);
        }

        if (IsStunned)
        {
            CanMove = false;
            _animator.SetBool("Stun", true);
        }

        if (!IsStunned)
        {
            _animator.SetBool("Stun", false);
        }

        if (player != null && !IsStunned)
        {
            transform.LookAt(player);
            ChasingPlayer();
        }
    }

    public void ChasingPlayer()
    {
        if (HP <= 0.5 * MaxHP && !IsRage)
        {
            IsRage = true;
            MaxCastDuration = 2f;
        }

        if (player != null && _navAgent != null)
        {
            if (IsStunned && !Charging)
            {
                _navAgent.isStopped = true;
                return;
            }
            else
            {
                _navAgent.isStopped = false;
            }

            cekRange();
            Charge();
            _navAgent.SetDestination(player.position);
            _navAgent.stoppingDistance = triggerRange;
        }
    }

    public void Charge()
    {
        if (Charging)
        {
            _animator.SetBool("isMoving", false);

            if (CastDuration > 0 && Charging)
            {
                // AudioManager.Instance.PlaySFXForDuration("Patih Charge", 2);
                _animator.SetTrigger("Charge");
                CastDuration -= Time.deltaTime;

                // Menambahkan inisialisasi Charge VFX
                if (chargeVFXInstance == null)
                {
                    chargeVFXInstance = Instantiate(ChargeVFX, rightHandTransform.position, rightHandTransform.rotation);
                    chargeVFXInstance.transform.SetParent(rightHandTransform);
                }
            }

            if (CastDuration <= 0f)
            {
                Attack();
                Charging = false;

                // Menghancurkan Charge VFX ketika Cast Duration selesai
                if (chargeVFXInstance != null)
                {
                    Destroy(chargeVFXInstance);
                }
            }
        }
    }

    public void cekRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= triggerRange && CanMove && !IsStunned)
        {
            Charging = true;
        }
    }

    public void Attack()
    {
        AudioManager.Instance.PlaySFX("Patih Att");
        _animator.SetTrigger("Attack");

        Vector3 attackDirection = player.position - transform.position;
        attackDirection.y = 0f;
        attackDirection.Normalize();

        Quaternion hitboxRotation = Quaternion.LookRotation(attackDirection);
        Vector3 startPoint = transform.position + transform.forward * 1f;
        Vector3 endPoint = startPoint + attackDirection * 7f;

        hitboxInstance = Instantiate(hitboxPrefab, startPoint, hitboxRotation);

        float hitboxLength = Vector3.Distance(startPoint, endPoint);

        // Calculate VFX scale based on hitbox length
        Vector3 vfxScale;

        if (!IsRage)
        {
            vfxScale = new Vector3(1f, 1f, 1f);
            hitboxInstance.transform.localScale = new Vector3(1f, 1f, hitboxLength);
        }
        else
        {
            vfxScale = new Vector3(3f, 1f, 1f);
            hitboxInstance.transform.localScale = new Vector3(3f, 1f, hitboxLength);
        }

        Vector3 vfxPosition = startPoint + new Vector3(0, 1, 0); // Adjust the y-value as needed
        GameObject vfxInstance = Instantiate(VFXAtt, vfxPosition, hitboxRotation);

        // Set VFX scale
        vfxInstance.transform.localScale = vfxScale;

        Renderer hitboxRenderer = hitboxInstance.GetComponent<Renderer>();
        if (hitboxRenderer != null)
        {
            hitboxRenderer.material.color = Color.red;
        }

        CanMove = false;

        StartCoroutine(StunEnemy());
        StartCoroutine(EnableMovementAfterCast());
    }

    IEnumerator EnableMovementAfterCast()
    {
        CastDuration = MaxCastDuration;
        yield return new WaitForSeconds(2f);
        CanMove = true;
    }

    IEnumerator StunEnemy()
    {
        yield return new WaitForSeconds(0.5f);

        if (hitboxInstance != null)
        {
            Destroy(hitboxInstance);
        }

        IsStunned = true;

        if (_playerHealth != null)
        {
            _playerHealth.TakeDamage(damage);
        }

        while (StunDuration > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            StunDuration -= Time.deltaTime;
        }

        StunDuration = 3f;

        IsStunned = false;
        CanMove = true;
    }



    public void SetHP()
    {
        BossHealthSlider.maxValue = MaxHP;
        BossHealthSlider.value = HP;
    }

    private void OnDestroy()
    {
        OnPatihDestroyed?.Invoke();
    }
}
