using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveScore : MonoBehaviour
{
    [SerializeField] private TMP_InputField scoreName;
    [SerializeField] private List<GameObject> scoreObjects = new List<GameObject>();
    private MakeScore scoreMaker;

    private void Start()
    {
        scoreMaker = FindFirstObjectByType<MakeScore>();
    }

    public void AddScore()
    {
        ScoreEntry newScore = new ScoreEntry(scoreName.text, scoreMaker.finalScore);
        
        ScoreFileHandler.SaveToJson(newScore);
        StartCoroutine(DisableAndFadeScoreInput(0.2f));
    }

    IEnumerator DisableAndFadeScoreInput(float duration)
    {
        scoreName.interactable = false;
        yield return new WaitForSecondsRealtime(duration);
        foreach (var scoreObject in scoreObjects)
        {
            scoreObject.SetActive(false);
        }
        scoreName.text = "";
    }
}
