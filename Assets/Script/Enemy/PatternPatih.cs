using UnityEngine;
using System.Collections;

public class PatternPatih : MonoBehaviour
{
    public Transform player; 
    public int damage, HP;
    
    public float moveSpeed = 3f;
    public float attackRange = 7f;
    public float triggerRange = 4f;
    public float CastDuration = 3f; 
    public float StunDuration = 3f; 

    public bool Charging = false;
    public bool CanMove = true;
    public bool IsStunned = false; 
    
    public GameObject hitboxPrefab; 
    public Material normalMaterial;
    public Material stunMaterial;

    private Health _playerHealth;
    private GameObject hitboxInstance; 
    void Start()
    {
        _playerHealth = GetComponent<Health>();
    }

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);

            ChasingPlayer();
        }
    }

    public void ChasingPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > triggerRange && CanMove && !IsStunned)
            {
                Vector3 direction = player.position - transform.position;
                direction.y = 0f;
                direction.Normalize();
                Vector3 movement = direction * moveSpeed * Time.deltaTime;
                transform.position += movement;
            }
            if (distanceToPlayer <= triggerRange && CanMove && !IsStunned)
            {
                Charging = true;
                CastDuration -= Time.deltaTime;
                if (CastDuration <= 0f)
                {
                    Attack();
                    Charging = false;
                }
            }
        }
    }

    public void Attack()
    {
        Vector3 attackDirection = player.position - transform.position;
        attackDirection.y = 0f;
        attackDirection.Normalize();

        // Tentukan titik awal hitbox di bagian depan musuh (wajah musuh)
        Vector3 startPoint = transform.position + transform.forward * 1f; // Misalnya, jarak hitbox dimulai dari 1 unit di depan musuh

        // Tentukan titik akhir hitbox yang berjarak 7 unit dari startPoint ke arah pemain
        Vector3 endPoint = startPoint + attackDirection * 7f;

        // Hitung rotasi hitbox agar menghadap ke arah pemain
        Quaternion hitboxRotation = Quaternion.LookRotation(attackDirection);

        // Buat instance dari hitbox area serangan
        hitboxInstance = Instantiate(hitboxPrefab, startPoint, hitboxRotation);

        // Tentukan panjang hitbox berdasarkan jarak antara startPoint dan endPoint
        float hitboxLength = Vector3.Distance(startPoint, endPoint);

        // Atur skala hitbox agar sesuai dengan panjang yang dihitung
        hitboxInstance.transform.localScale = new Vector3(1f, 1f, hitboxLength);

        Renderer hitboxRenderer = hitboxInstance.GetComponent<Renderer>();
        if (hitboxRenderer != null)
        {
            hitboxRenderer.material.color = Color.red;
        }

        CastDuration = 3f;
        CanMove = false;
        
        StartCoroutine(EnableMovementAfterCast());
    }

    IEnumerator EnableMovementAfterCast()
    {
        yield return new WaitForSeconds(CastDuration);

        if (hitboxInstance != null)
        {
            Destroy(hitboxInstance);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StunEnemy());

        CanMove = true;
    }
 

    IEnumerator StunEnemy()
    {
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

        if (enemyRenderer != null && normalMaterial != null)
        {
            enemyRenderer.material = normalMaterial;
        }

        IsStunned = false;
        CanMove = true;
}

}
