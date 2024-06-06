using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Health HealthScript;
    public Score ScoreScript;
    public Text ScoreText;
    public Text PriceForRevive;
    public GameObject GameOverScreen;
    public Button ReviveButton;
    public GameObject leaderboard;

    public int RevivePrice = 1000;
    private bool gameOver = false;

    private void Start()
    {
        GameObject playerKsatria = GameObject.Find("Player-Ksatria");
        GameObject playerDukun = GameObject.Find("Player_Dukun");
        GameObject playerPetani = GameObject.Find("Player_Petani");
        GameObject playerPrajurit = GameObject.Find("Player_Prajurit");
        if (playerKsatria != null)
        {
            HealthScript = playerKsatria.GetComponent<Health>();
        }
        else if (playerDukun != null)
        {
            HealthScript = playerDukun.GetComponent<Health>();
        }
        else if (playerPetani != null)
        {
            HealthScript = playerPetani.GetComponent<Health>();
        }
        else if (playerPrajurit != null)
        {
            HealthScript = playerPrajurit.GetComponent<Health>();
        }

        UpdateRevivePriceText();

        if (HealthScript == null)
        {
            HealthScript = FindObjectOfType<Health>();
        }

        if (ScoreScript == null)
        {
            ScoreScript = FindObjectOfType<Score>();
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

        UpdateReviveButtonInteractibility();
        UpdateScoreText();
        UpdateRevivePriceText();
    }

    private void GameOver()
    {
        // Tampilkan layar game over
        GameOverScreen.SetActive(true);
        PlayerPrefs.SetInt("CurrentScore", ScoreScript.currentScore);
        StaticScore.currentScore = ScoreScript.currentScore;
        Time.timeScale = 0;

        gameOver = true;
    }

    public void Revive()
    {
        Time.timeScale = 1;
        StaticScore.currentScore = 0;
        StaticScore.currentScore = ScoreScript.currentScore - RevivePrice;
    }

    public void UploadButton(){
        GameOverScreen.SetActive(false);
    }

    private void UpdateReviveButtonInteractibility()
    {
        // Jika skor mencapai 1000, aktifkan tombol Revive
        if (ScoreScript.currentScore >= RevivePrice)
        {
            ReviveButton.interactable = true;
        }
        // Jika tidak, non-aktifkan tombol Revive
        else
        {
            ReviveButton.interactable = false;
        }
    }

    private void UpdateScoreText()
    {
        // Update nilai teks menjadi nilai skor
        ScoreText.text = "Your Score: " + ScoreScript.currentScore.ToString();
    }

    private void UpdateRevivePriceText()
    {
        // Update nilai teks menjadi nilai harga revive
        PriceForRevive.text = "Revive Price: " + RevivePrice.ToString();
    }
}
