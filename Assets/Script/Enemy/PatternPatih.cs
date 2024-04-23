using UnityEngine;
using System.Collections;

public class PatternPatih : MonoBehaviour
{
    public Transform player; 
    public float moveSpeed = 5f;
    public float attackRange = 7f;
    public float triggerRange = 4f;
    public int damage, HP;

    public float CastDuration = 3f; 
  
    public bool Charging = false;
    public bool CanMove = true;
    private Health _playerHealth;

    public GameObject hitboxPrefab; // Prefab untuk visualisasi hitbox area serangan
    private GameObject hitboxInstance; // Instance dari hitbox area serangan

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
            if (distanceToPlayer > triggerRange && CanMove)
            {
                Vector3 direction = player.position - transform.position;
                direction.y = 0f;
                direction.Normalize();
                Vector3 movement = direction * moveSpeed * Time.deltaTime;
                transform.position += movement;
            }
            if (distanceToPlayer <= triggerRange && CanMove)
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

        // Dapatkan semua collider dalam area hitbox
        Collider[] colliders = Physics.OverlapBox(hitboxInstance.transform.position, hitboxInstance.transform.localScale / 2, hitboxRotation);

        // Iterasi melalui setiap collider yang didapat
        foreach (Collider collider in colliders)
        {
            // Cek apakah collider adalah pemain
            if (collider.CompareTag("Player"))
            {
                // Kurangi HP pemain jika memiliki komponen Health
                if (_playerHealth != null)
                {
                    _playerHealth.TakeDamage(damage); // Memanggil metode TakeDamage pada komponen Health
                }
            }
        }

        // Set CastDuration kembali ke nilai awal
        CastDuration = 3f;

        // Berhenti bergerak untuk durasi casting
        CanMove = false;
        // Mulai countdown untuk memungkinkan gerakan kembali setelah casting selesai
        StartCoroutine(EnableMovementAfterCast());
    }

    // Metode untuk mengaktifkan kembali gerakan setelah waktu casting selesai
    IEnumerator EnableMovementAfterCast()
    {
        yield return new WaitForSeconds(CastDuration);

        // Hapus instance hitbox
        if (hitboxInstance != null)
        {
            Destroy(hitboxInstance);
        }

        CanMove = true;
    }   
}
