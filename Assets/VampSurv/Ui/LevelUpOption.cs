using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpOption : MonoBehaviour
{
    public delegate void OptionSelected(GameObject option);
    public static event OptionSelected OnOptionSelected;
    
    [SerializeField] private Image optionImage;
    [SerializeField] private TMP_Text optionName;
    [SerializeField] private TMP_Text optionDescription;
    private GameObject m_option;
    
    public void SetOption(Sprite optImage, string optName, string optDescription, GameObject option)
    {
        m_option = option;
        optionImage.sprite = optImage;
        optionName.text = optName;
        optionDescription.text = optDescription;
    }
    
    public void ChoseOption()
    {
        OnOptionSelected?.Invoke(m_option);
    }
}
