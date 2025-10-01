using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelUpTextColorChange : MonoBehaviour
{
    private TMP_Text m_text;

    private void Start()
    {
        DOTween.Init();
        m_text = GetComponent<TMP_Text>();
        StartCoroutine(ChangeColorsDuringTime(Random.Range(0.3f, 0.6f)));
    }

    private IEnumerator ChangeColorsDuringTime(float time)
    {
        m_text.DOColor(Random.ColorHSV(0f,1f,1f,1f,1f,1f,1f,1f), time);
        yield return new WaitForSecondsRealtime(time);
        StartCoroutine(ChangeColorsDuringTime(Random.Range(0.3f, 0.6f)));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}