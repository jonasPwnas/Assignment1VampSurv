using System;
using Input;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    private float _rotationY;
    private float _rotationX;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);
    
    [SerializeField]
    AnimationCurve _smoothCurve;
    
    private CameraInputHandler  _cameraInputHandler;

    private void Start()
    {
        _cameraInputHandler = GetComponent<CameraInputHandler>();
    }

    void Update()
    {
        float mouseX = _cameraInputHandler.lookAction.ReadValue<Vector2>().x * _mouseSensitivity;
        float mouseY = _cameraInputHandler.lookAction.ReadValue<Vector2>().y * _mouseSensitivity * -1f;

        _rotationY += mouseX;
        _rotationX += mouseY;

        // Apply clamping for x rotation 
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);
        
        float evaluatedSmoothCurve = _smoothCurve.Evaluate(_cameraInputHandler.lookAction.ReadValue<Vector2>().magnitude);
        
        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        // Apply damping between rotation changes
        //_currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, evaluatedSmoothCurve);
        print("SmoothCurve: " + evaluatedSmoothCurve);
      
        transform.localEulerAngles = _currentRotation;

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = _target.position - transform.forward * _distanceFromTarget;
    }
}