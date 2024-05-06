using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class LeaderboardTest : MonoBehaviour
{
    public GameObject inputName;
    public GameObject inputField;
    public GameObject leaderdBoard;
    public GameObject Content;
    private Score scoreScript;
    private ScoreData sd;
    public GameObject RowUIPrefab;

    public void loadScore(){
        sd = new ScoreData();
    }

    void Start()
    {
        loadScore();
        scoreScript = GetComponent<Score>();
    }

    public void inputNameShow()
    {
        inputName.SetActive(true);
    }

    public IEnumerable<SaveScore> GetHighScores()
    {
        return sd.scoreList.OrderByDescending(x => x.Score);
    }

    public void AddScore(SaveScore ss)
    {
        sd.scoreList.Add(ss);
    }

    public void saveScore()
    {
        string name = inputField.GetComponent<InputField>().text;
        int score = scoreScript.currentScore;
        AddScore(new SaveScore(name,score));
        leaderdBoard.SetActive(true);
        inputName.SetActive(false);
        LeaderBoardShow();
    }

    public void LeaderBoardShow()
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