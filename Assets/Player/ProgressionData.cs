using UnityEngine;

[CreateAssetMenu(fileName = "ProgressionData", menuName = "Scriptable Objects/ProgressionData")]
public class ProgressionData : ScriptableObject
{
    [Tooltip("Time is Level and Value is Xp")] [SerializeField] AnimationCurve xpCurve;
    
    public float GetXpForLevel(float level)
    {
        return xpCurve.Evaluate(level);
    }
}
