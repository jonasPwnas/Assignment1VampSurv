using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpTextColorChange : MonoBehaviour
{
    private TMP_Text m_text;

    private void Start()
    {
        m_text = GetComponent<TMP_Text>();
    }
    
    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(1f);
        m_text.color = Color.white;
    }
}
