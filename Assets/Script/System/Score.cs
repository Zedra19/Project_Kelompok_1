using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    //Scoring
    public int currentScore = 0;
    public int EnemyS = 100;
    public int EnemyM = 250;
    public int EnemyL = 500;
    public int Boss = 1000;

    //UI
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        currentScore = StaticScore.currentScore;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + currentScore;
    }

    public void BuyUpgrade(int price)
    {
        if (currentScore >= price)
        {
            currentScore -= price;
            StaticScore.currentScore = currentScore;
        }
        else
        {
            Debug.Log("Not enough score!");
        }
    }
}
