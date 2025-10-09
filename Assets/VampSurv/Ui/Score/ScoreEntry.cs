using System;

[Serializable]
public class ScoreEntry
{
    public string scoreName;
    public int scoreScore;
    
    public ScoreEntry(string scoreName, int scoreScore)
    {
        this.scoreName = scoreName;
        this.scoreScore = scoreScore;
    }
}