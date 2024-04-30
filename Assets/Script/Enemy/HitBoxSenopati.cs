using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Periksa jika objek yang masuk ke dalam hitbox adalah pemain
        if (other.CompareTag("Player"))
        {
            // Dapatkan komponen Health dari pemain
            Health playerHealth = other.GetComponent<Health>();
            
            // Periksa apakah komponen Health diperoleh dengan benar
            if (playerHealth != null)
            {
                Debug.Log("Player Hit");
                
                // Sebelum mengakses metode TakeDamage, pastikan metode tersebut didefinisikan di komponen Health
                playerHealth.TakeDamage(1);
                
                // Hancurkan objek hitbox setelah menyentuh pemain
                Destroy(gameObject);
            }
        }
    }
}
