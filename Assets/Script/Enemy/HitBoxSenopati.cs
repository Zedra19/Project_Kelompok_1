using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Jika terjadi tabrakan dengan objek yang memiliki tag "Player"
        if (other.CompareTag("Player"))
        {
            // Hancurkan hitbox
            Destroy(gameObject);
        }
    }
}
