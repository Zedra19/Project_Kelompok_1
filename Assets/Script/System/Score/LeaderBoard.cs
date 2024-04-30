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
    public RowUI rowUIScript;

    void Awake()
    {
        var json = PlayerPrefs.GetString("Leaderboard", "{}");
        sd = JsonUtility.FromJson<ScoreData>(json);
    }
    // Start is called before the first frame update
    void Start()
    {
        scoreScript = GetComponent<Score>();
    }
    void Update()
    {
        leaderBoardShow();
    }

    public IEnumerable<SaveScore> GetHighScores()
    {
        return sd.scoreList.OrderByDescending(x => x.Score);
    }

    public void AddScore(SaveScore ss)
    {
        sd.scoreList.Add(ss);
    }

    public void inputNameShow()
    {
        inputName.SetActive(true);
    }

    public void leaderBoardShow()
    {
        if (leaderdBoard.activeSelf)
        {
            var scores = GetHighScores().ToArray();
            for (int i = 0; i < scores.Length; i++)
            {
                var row = Instantiate(rowUIScript, Content.transform).GetComponent<RowUI>();
                row.Rank.text = (i + 1).ToString();
                row.Name.text = scores[i].Name;
                row.Score.text = scores[i].Score.ToString();
            }
        }
    }

    public void saveScore()
    {
        string name = inputField.GetComponent<InputField>().text;
        int score = scoreScript.currentScore;
        Debug.Log("Name: " + name + " Score: " + score);
        AddScore(new SaveScore(name, score));
        leaderdBoard.SetActive(true);
        inputName.SetActive(false);
    }

    public void homeButton()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveScoreToLeaderBoard()
    {
        var json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("Leaderboard", json);
    }
}
