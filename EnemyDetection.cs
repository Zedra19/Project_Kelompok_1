using System.Collections;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float DetectionRange = 0f;
    public float AttackRange = 0f;
    public float ChaseSpeed = 0f;
    public float KnockbackForce = 0f;
    public float CooldownDuration = 0f;
    public string PlayerTag = "Player";

    private Transform _playerTransform;
    private bool _isOnCooldown = false;

    void Update()
    {
        // Cari objek pemain berdasarkan tag
        if (_playerTransform == null)
        {
            GameObject playerobject = GameObject.FindGameObjectWithTag(PlayerTag);

            // Periksa apakah pemain telah ditemukan
            if (playerobject != null)
            {
                _playerTransform = playerobject.transform;
            }
            // Jika pemain tidak ditemukan, tampilkan pesan peringatan
            else
            {
                Debug.LogWarning("Player object not found!");
                return;
            }
        }

        // Hitung jarak antara musuh dan pemain
        float distancetoplayer = Vector3.Distance(transform.position, _playerTransform.position);

        // Jika pemain berada dalam jarak deteksi, musuh mulai mengejar pemain
        if (distancetoplayer <= DetectionRange)
        {
            // Jika musuh berada di luar jarak serangan, lanjutkan mengejar pemain
            if (distancetoplayer > AttackRange)
            {
                ChasePlayer();
            }
            else
            {
                // Jika musuh berada dalam jarak serangan dan tidak dalam cooldown, serang pemain
                if (!_isOnCooldown)
                {
                    AttackPlayer();
                }
            }
        }
    }

    // Fungsi untuk mengejar pemain
    void ChasePlayer()
    {
        Vector3 movedirection = (_playerTransform.position - transform.position).normalized;
        transform.position += movedirection * ChaseSpeed * Time.deltaTime;
    }

    // Fungsi untuk menyerang pemain
    void AttackPlayer()
    {
        Debug.Log("Damage");
        
        // Beri knockback pada pemain
        Vector3 knockbackdirection = (_playerTransform.position - transform.position).normalized;
        _playerTransform.GetComponent<Rigidbody>().AddForce(knockbackdirection * KnockbackForce, ForceMode.Impulse);

        // Berikan damage pada pemain
        // _playerTransform.GetComponent<PlayerHealth>().TakeDamage(Damage);

        // Mulai cooldown
        StartCoroutine(StartCooldown());
    }

    // Coroutine untuk memulai cooldown
    IEnumerator StartCooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(CooldownDuration);
        _isOnCooldown = false;
    }
}
