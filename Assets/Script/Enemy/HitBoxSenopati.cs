using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Player Hit");
                playerHealth.TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }
}
