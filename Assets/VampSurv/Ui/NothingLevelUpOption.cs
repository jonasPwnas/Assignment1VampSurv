using UnityEngine;

public class NothingLevelUpOption : MonoBehaviour
{
    public delegate void ChoseNothingLevelUpOption();
    public static event ChoseNothingLevelUpOption OnChoseNothingLevelUpOption;

    public void ChoseNothing()
    {
        OnChoseNothingLevelUpOption?.Invoke();
    }
}
