using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardTest : MonoBehaviour
{
    [System.Serializable]
    public class LeaderboardEntry
    {
        public int Rank;
        public string Name;
        public int Score;
    }

    public Text RankText;
    public Text NameText;
    public Text ScoreText;
    public GameObject EntryPrefab;
    public Transform Content;

    private List<LeaderboardEntry> leaderboard;

    private void Start()
    {
        leaderboard = new List<LeaderboardEntry>
        {
            new LeaderboardEntry { Rank = 1, Name = "Asep", Score = 1000 },
            new LeaderboardEntry { Rank = 2, Name = "Budi", Score = 800 },
            new LeaderboardEntry { Rank = 3, Name = "Caca", Score = 600 },
            new LeaderboardEntry { Rank = 4, Name = "Doni", Score = 400 },
            new LeaderboardEntry { Rank = 5, Name = "Eko", Score = 200 },
        };

        foreach (var entry in leaderboard)
        {
            var newEntry = Instantiate(EntryPrefab, Content);
            newEntry.GetComponent<LeaderboardEntryUI>().SetValues(entry.Rank, entry.Name, entry.Score);
        }
    }
}

[RequireComponent(typeof(LeaderboardEntryUI))]
public class LeaderboardEntryUI : MonoBehaviour
{
    public Text RankText;
    public Text NameText;
    public Text ScoreText;

    public void SetValues(int rank, string name, int score)
    {
        RankText.text = rank.ToString();
        NameText.text = name;
        ScoreText.text = score.ToString();
    }
}
