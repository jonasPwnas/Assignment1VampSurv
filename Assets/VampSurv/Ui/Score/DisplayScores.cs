using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DisplayScores : MonoBehaviour
{
    [SerializeField] private GameObject scoresHolder;
    [SerializeField] private GameObject scorePrefab;

    private void OnEnable()
    {
        DisplayScore();
        print("This one we're doing ALRIGHT!!!");
    }
    
    private void DisplayScore()
    {
        List<ScoreEntry> scores = ScoreFileHandler.ReadFromJson<ScoreEntry>();

        scores.Sort((a, b) => b.scoreScore.CompareTo(a.scoreScore));
        
        foreach (ScoreEntry score in scores)
        {
            GameObject scoreObject = Instantiate(scorePrefab, scoresHolder.transform);
            scoreObject.GetComponent<PopulateScoreObject>().PopulateObject(score.scoreName, score.scoreScore);
        }
    }
}
