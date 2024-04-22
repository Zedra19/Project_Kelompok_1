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
  
    public bool CanMove = true;
    private Health _playerHealth;

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
                CastDuration -= Time.deltaTime;
                while (CastDuration < 3)
                    if (CastDuration <= 0f)
                    {
                        Attack();
                    }
            }
        }
    }

    public void Attack()
    {
        // Tentukan area serangan
        Vector3 boxCenter = transform.position + transform.forward * 3.5f; // Mengasumsikan panjang trisula adalah 7, jadi setengahnya adalah 3.5
        Vector3 boxSize = new Vector3(1f, 1f, 7f); // Lebar 1 dan Tinggi 1 (dapat disesuaikan), Panjang 7

        // Dapatkan semua collider dalam area kotak
        Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize / 2);

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
        CanMove = true;
    }   
}
