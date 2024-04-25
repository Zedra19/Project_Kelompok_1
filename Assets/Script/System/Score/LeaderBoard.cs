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
    public Score scoreScript;

    public Text rank1;
    public Text rank2;
    public Text rank3;
    public Text rank4;
    public Text rank5;


    // Start is called before the first frame update
    void Start()
    {
        scoreScript = FindAnyObjectByType<Score>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowLeaderBoard();
    }

    void ShowLeaderBoard()
    {
        if (leaderdBoard.activeSelf == true){
            rank1.text = (1) + " " + PlayerPrefs.GetString(name);
        }
    }

    public void inputNameShow()
    {
        inputName.SetActive(true);
    }

    public void LeaderBoardShow()
    {
        leaderdBoard.SetActive(true);
    }

    public void saveScore(){
        string name = inputField.GetComponent<InputField>().text;
        int score = scoreScript.currentScore;
        PlayerPrefs.SetString(name, score.ToString());
        LeaderBoardShow();
        inputName.SetActive(false);
    }

    public void homeButton(){
        SceneManager.LoadScene(0);
    }
}
