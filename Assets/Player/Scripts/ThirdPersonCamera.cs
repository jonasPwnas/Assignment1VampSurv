using System;
using Input;
using Player;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3.0f;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private float stationaryDistanceFromTarget = 6.0f;
    [SerializeField] private float movingDistanceFromTarget = 8.5f;
    [SerializeField] private float heightOffset = 1f;
    [SerializeField] private float distanceSmoothTime = 0.2f;
    [SerializeField] private Vector2 rotationXMinMax = new Vector2(-7, 40);
    [SerializeField] ScriptableFloatCurve smoothCurve;

    private float m_rotationY;
    private float m_rotationX;
    private Vector3 m_currentRotation;
    private Vector3 m_smoothVelocity = Vector3.zero;
    private float m_distanceToTarget;
    private float m_targetDistanceToTarget;
    private float m_refVelocity;
    private CameraInputHandler  m_cameraInputHandler;

    private void Start()
    {
        m_cameraInputHandler = GetComponent<CameraInputHandler>();
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = m_cameraInputHandler.lookAction.ReadValue<Vector2>().x * mouseSensitivity;
        float mouseY = m_cameraInputHandler.lookAction.ReadValue<Vector2>().y * mouseSensitivity * -1f;

        m_rotationY += mouseX;
        m_rotationX += mouseY;

        // Apply clamping for x rotation 
        m_rotationX = Mathf.Clamp(m_rotationX, rotationXMinMax.x, rotationXMinMax.y);
        
        float evaluatedSmoothCurve = smoothCurve.floatCurve.Evaluate(m_cameraInputHandler.lookAction.ReadValue<Vector2>().magnitude);
        
        Vector3 nextRotation = new Vector3(m_rotationX, m_rotationY);

        // Apply damping between rotation changes
        m_currentRotation = Vector3.SmoothDamp(m_currentRotation, nextRotation, ref m_smoothVelocity, evaluatedSmoothCurve);
      
        transform.localEulerAngles = m_currentRotation;
        float distanceMoveVelocity = player._moveVelocity.normalized.magnitude;
        
        if (distanceMoveVelocity > 0.01f)
        {
            m_targetDistanceToTarget = movingDistanceFromTarget;
        }
        else
        {
            m_targetDistanceToTarget = stationaryDistanceFromTarget;
        }
        
        m_distanceToTarget = Mathf.SmoothDamp(m_distanceToTarget, m_targetDistanceToTarget, ref m_refVelocity, distanceSmoothTime);
        
        // Subtract the forward vector of the GameObject to point its forward vector to the target
        transform.position = player.transform.position - transform.forward * m_distanceToTarget + transform.up * heightOffset;
    }
}