using System;
using UnityEngine;

public class SimpleCameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform _trackingTarget;
    [SerializeField] private float _trackingSpeed = 0.5f;
    
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _trackingTarget.position, _trackingSpeed * Time.deltaTime);
    }
}
