using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PopulateScoreObject : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreName;
    [SerializeField] private TMP_Text scoreScore;

    public void PopulateObject(string name, int score)
    {
        scoreName.text = name;
        scoreScore.text = score.ToString();
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}