using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
    public GameObject leaderdBoard;
    public GameObject Content;
    public Score scoreScript;

    private ScoreData sd;
    private ScoreDataEndless sde;
    public bool isEndless = false;
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
        string leaderboardEndlessJson = PlayerPrefs.GetString("LeaderboardEndless");
        if (!string.IsNullOrEmpty(leaderboardJson))
        {
            sde = JsonUtility.FromJson<ScoreDataEndless>(leaderboardEndlessJson);
        }
        else
        {
            sde = new ScoreDataEndless();
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        loadScore();
        scoreScript = GetComponent<Score>();
    }

    public IEnumerable<SaveScore> GetHighScores()
    {
        return sd.scoreList.OrderByDescending(x => x.Score);
    }

    public IEnumerable<SaveScore> GetHighScoresEndless()
    {
        return sde.scoreList.OrderByDescending(x => x.Score);
    }

    public void AddScore(SaveScore ss)
    {
        if(isEndless == false){
            sd.scoreList.Add(ss);
        }
        else if(isEndless == true){
            sde.scoreList.Add(ss);
        }
    }

    public void saveScore()
    {
        string name = PlayerPrefs.GetString("Profile"+Profile.CurrentPlayerIndex);
        int score = scoreScript.currentScore;
        AddScore(new SaveScore(name, score));
        SaveLeaderboard();
        leaderBoardShow();
        leaderdBoard.SetActive(true);
    }

    public void SaveLeaderboard()
    {
        if(isEndless == false){
            string json = JsonUtility.ToJson(sd);
            PlayerPrefs.SetString("Leaderboard", json);
        }else if(isEndless == true){
            string json = JsonUtility.ToJson(sde);
            PlayerPrefs.SetString("LeaderboardEndless", json);
        }
    }

    public void leaderBoardShow()
    {
        if(isEndless == false){
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }
            var scores = GetHighScores().Take(10).ToArray();
            for (int i = 0; i < scores.Length; i++)
            {
                var row = Instantiate(RowUIPrefab, Content.transform).GetComponent<RowUI>();
                row.Rank.text = (i + 1).ToString();
                row.Name.text = scores[i].Name;
                row.Score.text = scores[i].Score.ToString();
            }
        }else if(isEndless == true){
            foreach (Transform child in Content.transform)
            {
                Destroy(child.gameObject);
            }
            var scores = GetHighScoresEndless().Take(10).ToArray();
            for (int i = 0; i < scores.Length; i++)
            {
                var row = Instantiate(RowUIPrefab, Content.transform).GetComponent<RowUI>();
                row.Rank.text = (i + 1).ToString();
                row.Name.text = scores[i].Name;
                row.Score.text = scores[i].Score.ToString();
            }
        }
    }

    public void homeButton()
    {
        StaticScore.currentScore = 0;
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
