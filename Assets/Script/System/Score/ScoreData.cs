using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData : MonoBehaviour
{
    public List<SaveScore> scoreList;
    
    public ScoreData() {
        scoreList = new List<SaveScore>();
    }
}
