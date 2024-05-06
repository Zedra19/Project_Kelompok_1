using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public List<SaveScore> scoreList;
    
    public ScoreData() {
        scoreList = new List<SaveScore>();
    }
}
