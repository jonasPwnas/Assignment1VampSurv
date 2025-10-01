using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpOption : MonoBehaviour
{
    [SerializeField] private Image optionImage;
    [SerializeField] private TMP_Text optionName;
    [SerializeField] private TMP_Text optionDescription;
    
    public void SetOption(Sprite optImage, string optName, string optDescription)
    {
        optionImage.sprite = optImage;
        optionName.text = optName;
        optionDescription.text = optDescription;
    }
}
