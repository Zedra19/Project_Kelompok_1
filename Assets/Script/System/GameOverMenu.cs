using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Health HealthScript;
    public GameObject GameOverScreen;
    private bool gameOver = false;

    private void Start()
    {
        if (HealthScript == null)
        {
            HealthScript = FindObjectOfType<Health>();
        }
    }

    private void Update()
    {
        // Cek apakah nyawa habis dan game belum berakhir
        if (!gameOver && HealthScript != null && HealthScript.currentHealth <= 0)
        {
            Debug.Log("Player is Dead!");
            GameOver();
        }
    }

    private void GameOver()
    {
        // Tampilkan layar game over
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;

        gameOver = true;
    }

    public void Revive()
    {
        Time.timeScale = 1;
    }
}
