using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class PatternPatih : MonoBehaviour
{
    private Animator _animator;
    public int damage, HP, MaxHP;

    public float moveSpeed = 5f;
    public float attackRange = 7f;
    public float triggerRange = 4f;
    public float CastDuration = 2f;
    public float StunDuration = 3f;

    public bool Charging = false;
    public bool CanMove = true;
    public bool IsStunned = false;
    public bool IsRage = false;
    public bool RageAnim = false;

    public GameObject VFXAtt;
    public GameObject hitboxPrefab;
    public Material normalMaterial;
    public Material stunMaterial;
    public Material rageMaterial;
    public BossHealth bossHealth;

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
        _animator = GetComponent<Animator>();
        HP = bossHealth.BossMaxHealth;
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
        _playerHealth = GetComponent<Health>();
        _navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _navAgent.speed = moveSpeed;

        if (IsRage)
        {
            Renderer enemyRenderer = GetComponent<Renderer>();
            enemyRenderer.material = rageMaterial;
        }
    }

    void Update()
    {
        CekBool();
    }

    public void CekBool()
    {
        if(!RageAnim && IsRage)
        {
            RageAnim = true;
            _animator.SetTrigger("Rage");
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
                _animator.SetTrigger("Charge");
                CastDuration -= Time.deltaTime;
            }
            if (CastDuration <= 0f)
            {
                Attack();
                Charging = false;
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
        CastDuration = 3f;
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

        Renderer enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null && stunMaterial != null)
        {
            enemyRenderer.material = stunMaterial;
        }

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

        if (!IsRage)
        {
            enemyRenderer.material = normalMaterial;
        }
        else
        {
            enemyRenderer.material = rageMaterial;
        }

        IsStunned = false;
        CanMove = true;
    }
}
