using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TwoBoneTargetMover : MonoBehaviour
{
    [SerializeField] private ChainIKConstraint constraint;
    [SerializeField] private Transform constraintTarget;
    [SerializeField] private Transform target;

    private float m_weight = 1f;
    private float m_smoothVelocityRef = 0.0f;
    private const float SMOOTH_TIME = 0.1f;
    
    private void Update()
    {
        constraint.weight = Mathf.SmoothDamp(constraint.weight, m_weight, ref m_smoothVelocityRef, SMOOTH_TIME);
        constraint.weight = 1f;
        
        constraintTarget.position = target.position;
        constraintTarget.rotation = target.rotation;
    }

    public void SetTwoBoneIkWeight(float weight)
    {
        m_weight = weight;
    }
} 