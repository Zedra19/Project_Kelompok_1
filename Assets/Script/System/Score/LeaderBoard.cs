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

    // Start is called before the first frame update
    void Start()
    {
        sd = new ScoreData();
        var json = PlayerPrefs.GetString("Leaderboard");
        if (IsValidJson(json))
        {
            Debug.Log("CHECK JSON : "+json);
            sd = JsonUtility.FromJson<ScoreData>(json);
        }
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
        Debug.Log("Name: " + name + " Score: " + score);
        AddScore(new SaveScore(name,score));
        SaveScoreToLeaderBoard();
        leaderdBoard.SetActive(true);
        inputName.SetActive(false);
        Debug.Log("Leaderboard saved");
    }

    public void SaveScoreToLeaderBoard()
    {
        sd.scoreList.ForEach(x => Debug.Log("Name: " + x.Name + " Score: " + x.Score));
        var json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("Leaderboard", json);
    }

    public void leaderBoardShow()
    {
        if (leaderdBoard.activeSelf)
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
    }

    public void homeButton()
    {
        SceneManager.LoadScene(0);
    }
    
    private bool IsValidJson(string jsonString)
    {
        try
        {
            JsonUtility.FromJsonOverwrite(jsonString, new object());
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}
