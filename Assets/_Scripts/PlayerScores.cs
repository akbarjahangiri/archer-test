using System.Collections.Generic;

[System.Serializable]
public class PlayerScores
{
    public List<PlayerScoreEntry> scores = new List<PlayerScoreEntry>();
}

[System.Serializable]
public class PlayerScoreEntry
{
    public string name;
    public int score = 0;
}