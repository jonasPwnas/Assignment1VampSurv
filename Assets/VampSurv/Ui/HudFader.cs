using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HudFader : MonoBehaviour
{
    public delegate void FadeComplete(bool fadeIn);
    public static event FadeComplete OnFadeComplete;
    
    [SerializeField] private Image faderImage;
    
    public void StartFade(float duration, bool fadeIn, float delay)
    {
        float targetAlpha = fadeIn ? 0f : 1f;
        StartCoroutine(FadeImage(duration, targetAlpha, fadeIn, delay));
    }

    IEnumerator FadeImage(float duration, float targetAlpha, bool fadeIn, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        faderImage.DOFade(targetAlpha, duration);
        yield return new WaitForSecondsRealtime(duration);
        OnFadeComplete?.Invoke(fadeIn);
    }
}
