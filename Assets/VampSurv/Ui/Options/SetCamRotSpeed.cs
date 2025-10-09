using System;
using UnityEngine;
using UnityEngine.UI;

public class SetCamRotSpeed : MonoBehaviour
{
    public delegate void CamRotSpeedChanged();
    public static event CamRotSpeedChanged OnCamRotSpeedChanged;
    
    [SerializeField] private Slider camRotSpeedSlider;
    [SerializeField] private Toggle usingControllerToggle;
    [SerializeField] private ScriptablePlayerPrefsKeys playerPrefsKeys;
    
    private float m_camRotSpeed;
    private float m_newRotSpeed;
    private float m_camRotSpeedMax = 0.8f;
    private float m_camRotSpeedMin = 0.01f;
    private int m_usingController = 0;
    
    public void SetNewSpeed(float speed)
    {
        m_newRotSpeed = Mathf.Lerp(m_camRotSpeedMin, m_camRotSpeedMax, speed);
        print("new cam rot speed: " + m_newRotSpeed);
    }

    public void SetUsingController(bool usingController)
    {
        if (usingController)
        {
            m_usingController = 1;
        }
        else
        {
            m_usingController = 0;
        }
    }
    
    private void OnEnable()
    {
        m_camRotSpeed = PlayerPrefs.GetFloat(playerPrefsKeys.cameraRotationSpeedKey);
        if (PlayerPrefs.GetInt(playerPrefsKeys.isUsingControllerKey) == 1)
        {
            usingControllerToggle.isOn = true;
        }
        else
        {
            usingControllerToggle.isOn = false;
        }
        
        camRotSpeedSlider.value = Mathf.InverseLerp(m_camRotSpeedMin, m_camRotSpeedMax, m_camRotSpeed);
        usingControllerToggle.onValueChanged.AddListener(SetUsingController);
    }

    private void OnDisable()
    {
        usingControllerToggle.onValueChanged.RemoveListener(SetUsingController);
        if (m_usingController == 1)
        {
            m_camRotSpeed = m_newRotSpeed * 3f; //wow
        }
        else
        {
            m_camRotSpeed = m_newRotSpeed;
        }
        PlayerPrefs.SetFloat(playerPrefsKeys.cameraRotationSpeedKey, m_camRotSpeed);
        PlayerPrefs.SetInt(playerPrefsKeys.isUsingControllerKey, m_usingController);
        OnCamRotSpeedChanged?.Invoke();
    }
}
