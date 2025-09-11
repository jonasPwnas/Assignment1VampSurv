using System;
using UnityEngine;

namespace Utility
{
    public class SmoothMoveToTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _smoothTime = 0.2f;

        private Vector3 _lastPosition;

        private void Start()
        {
            _lastPosition = transform.position;
        }

        void Update()
        {
            if (transform.position != _target.position)
            {
                Vector3 currentVelocity = transform.position - _lastPosition;
                transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref currentVelocity, _smoothTime, _speed, Time.deltaTime);
                _lastPosition = transform.position;
            }
        }
    }
}
