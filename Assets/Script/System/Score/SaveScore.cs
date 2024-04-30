using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveScore : MonoBehaviour
{
    public string Name;
    public int Score;

    public SaveScore(string name, int score)
    {
        Debug.Log("SaveScore");
        Debug.Log("Name: " + name + " Score: " + score);
        Name = name;
        Score = score;
    }
}
