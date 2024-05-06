using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
    public GameObject inputName;
    public GameObject inputField;
    public GameObject leaderdBoard;
    public GameObject Content;
    public Score scoreScript;

    private ScoreData sd;
    public GameObject RowUIPrefab;

    public void loadScore()
    {
        string leaderboardJson = PlayerPrefs.GetString("Leaderboard");
        if (!string.IsNullOrEmpty(leaderboardJson))
        {
            sd = JsonUtility.FromJson<ScoreData>(leaderboardJson);
        }
        else
        {
            sd = new ScoreData();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        loadScore();
        scoreScript = GetComponent<Score>();
    }

    public IEnumerable<SaveScore> GetHighScores()
    {
        return sd.scoreList.OrderByDescending(x => x.Score);
    }

    public void inputNameShow()
    {
        inputName.SetActive(true);
    }

    public void AddScore(SaveScore ss)
    {
        sd.scoreList.Add(ss);
    }

    public void saveScore()
    {
        string name = inputField.GetComponent<InputField>().text;
        int score = scoreScript.currentScore;
        AddScore(new SaveScore(name, score));
        SaveLeaderboard();
        leaderBoardShow();
        leaderdBoard.SetActive(true);
        inputName.SetActive(false);
    }
    
    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("Leaderboard", json);
    }

    public void leaderBoardShow()
    {
        var scores = GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(RowUIPrefab, Content.transform).GetComponent<RowUI>();
            row.Rank.text = (i + 1).ToString();
            row.Name.text = scores[i].Name;
            row.Score.text = scores[i].Score.ToString();
        }
    }

    public void homeButton()
    {
        SceneManager.LoadScene(0);
    }
}
