using System;
using TMPro;
using UnityEngine;

public class MakeScore : MonoBehaviour
{
    [SerializeField] private HudUpdater hudUpdater;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text timeText;
    [HideInInspector] public int finalScore = 44;
    
    private int m_kills;
    private float m_time;
    
    private void OnEnable()
    {
        GetScoreData();
    }

    private void GetScoreData()
    {
        m_kills = hudUpdater.totalKills;
        m_time = hudUpdater.totalTime;
        
        timeText.text = GetFormatedTime(m_time);;
        killsText.text = m_kills.ToString(format:"0000");
        scoreText.text = GetFormatedScore(m_time, m_kills);
    }
    
    private string GetFormatedTime(float timeToDisplay)
    {
        float minutesTime = Mathf.FloorToInt(timeToDisplay / 60);
        float secondsTime = Mathf.FloorToInt(timeToDisplay % 60);
        
        return minutesTime.ToString("00") + ":" + secondsTime.ToString("00");
    }

    private string GetFormatedScore(float time, int kills)
    {
        if (kills == 0)
            kills = 1;
        
        
        int score = (int)time * kills + 666;
        finalScore = score;
        return score.ToString("0000000");
    }
}
