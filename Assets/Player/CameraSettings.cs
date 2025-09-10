using System;
using Input;
using Unity.Mathematics;
using UnityEngine;

public class SimpleCameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform trackingTarget;
    [SerializeField] private float trackingSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 0.5f;
    
    private CameraInputHandler  _cameraInputHandler;
    private Vector2 m_mouseDelta;
    private float m_rotationThreshold = 0.2f;
    private const float InternalRotationSpeed = 4f;
    private Quaternion m_horizontalRotationTarget;
    private Quaternion m_verticalRotationTarget;

    private void Start()
    {
        _cameraInputHandler = GetComponent<CameraInputHandler>();
        m_horizontalRotationTarget = transform.rotation;
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, trackingTarget.position, trackingSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        
        m_mouseDelta = _cameraInputHandler.lookAction.ReadValue<Vector2>();
        
        //Maybe move to an event as in the documentation, so it accumulates properly?
        //m_rotationTarget *= Quaternion.AngleAxis(m_mouseDelta.x * Time.deltaTime * rotationSpeed, Vector3.up);
        m_horizontalRotationTarget *= Quaternion.AngleAxis(m_mouseDelta.x * Time.deltaTime * rotationSpeed, Vector3.up);
        //m_verticalRotationTarget *= Quaternion.AngleAxis(m_mouseDelta.y * Time.deltaTime * rotationSpeed, Vector3.left);
        //Apply the rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, m_horizontalRotationTarget, InternalRotationSpeed * Time.deltaTime);
    }
    
    private void Update()
    {
        MoveCamera();
        RotateCamera();
    }
}
