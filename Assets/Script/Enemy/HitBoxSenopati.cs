using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Transform _playerTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = _playerTransform.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Player Hit");
                playerHealth.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}
