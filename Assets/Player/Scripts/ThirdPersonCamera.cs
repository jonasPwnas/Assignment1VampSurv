using System;
using Input;
using Player;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField] private PlayerMovement _player;

    [SerializeField] private float _stationaryDistanceFromTarget = 6.0f;
    [SerializeField] private float _movingDistanceFromTarget = 8.5f;
    [SerializeField] private float _heightOffset = 1f;
    
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    private float distanceToTarget;
    private float targetDistanceToTarget;
    private float nollan; //???

    [SerializeField] private float _distanceSmoothTime = 0.2f;
    [SerializeField] private Vector2 _rotationXMinMax = new Vector2(-20, 40);
    [SerializeField] ScriptableFloatCurve _smoothCurve;
    
    private CameraInputHandler  _cameraInputHandler;

    private void Start()
    {
        _cameraInputHandler = GetComponent<CameraInputHandler>();
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = _cameraInputHandler.lookAction.ReadValue<Vector2>().x * _mouseSensitivity;
        float mouseY = _cameraInputHandler.lookAction.ReadValue<Vector2>().y * _mouseSensitivity * -1f;

        _rotationY += mouseX;
        _rotationX += mouseY;

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);
        
        float evaluatedSmoothCurve = _smoothCurve.floatCurve.Evaluate(_cameraInputHandler.lookAction.ReadValue<Vector2>().magnitude);
        
        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, evaluatedSmoothCurve);
      
        transform.localEulerAngles = _currentRotation;
        float distanceMoveVelocity = _player._moveVelocity.normalized.magnitude;
        
        if (distanceMoveVelocity > 0.01f)
        {
            targetDistanceToTarget = _movingDistanceFromTarget;
        }
        else
        {
            targetDistanceToTarget = _stationaryDistanceFromTarget;
        }
        
        distanceToTarget = Mathf.SmoothDamp(distanceToTarget, targetDistanceToTarget, ref nollan, _distanceSmoothTime);
        
        // Subtract the forward vector of the GameObject to point its forward vector to the target
        transform.position = _player.transform.position - transform.forward * distanceToTarget + transform.up * _heightOffset; //Stuttering comes from this FIX IT!!!!
    }
}