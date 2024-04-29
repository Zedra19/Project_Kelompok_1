using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LeaderboardTest : MonoBehaviour
{
    public GameObject inputName;
    public GameObject inputField;
    public GameObject leaderdBoard;
    public GameObject Content;
    public Score scoreScript;
    public Text[] rowTexts;

    List<SaveScore> saveScores = new List<SaveScore>();

    void Start()
    {
        scoreScript = GetComponent<Score>();
        LoadScoreFromLeaderBoard();
    }

    public void inputNameShow()
    {
        inputName.SetActive(true);
    }

    public void saveScore()
    {
        string name = inputField.GetComponent<InputField>().text;
        int score = scoreScript.currentScore;
        saveScores.Add(new SaveScore(name, score));
        leaderdBoard.SetActive(true);
        inputName.SetActive(false);
        SaveScoreToLeaderBoard();
        
    }

    public void LoadScoreFromLeaderBoard()
    {
        string json = PlayerPrefs.GetString("Leaderboard");
        saveScores = JsonUtility.FromJson<List<SaveScore>>(json);
    }

    public void SaveScoreToLeaderBoard()
    {
        string json = JsonUtility.ToJson(saveScores);
        PlayerPrefs.SetString("Leaderboard", json);
    }

    public void leaderBoardShow(){
        if(leaderdBoard.activeSelf){
            var scores = saveScores.OrderByDescending(x => x.Score).ToArray();
            for (int i = 0; i < scores.Length; i++)
            {
                var row = Instantiate(rowTexts[i], Content.transform);
                row.text = (i+1) + ". " + scores[i].Name + " : " + scores[i].Score;
            }
        }
    }

    public void homeButton(){
        SceneManager.LoadScene(0);
    }
}