using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamPan : MonoBehaviour
{
    [SerializeField] List<Transform> targets;
    [SerializeField] Transform lookAtTarget;
    
    private Transform m_currentTarget;
    private int m_targetIndex = 1;
    
    private void Start()
    {
        m_currentTarget = targets[m_targetIndex];
    }

    private void Update()
    {
        transform.LookAt(lookAtTarget);
        
        if (transform.position != m_currentTarget.position)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, m_currentTarget.position, 100f * Time.deltaTime);
        }
        else
        {
            if (m_targetIndex >= targets.Count)
            {
                m_targetIndex = 0;
                m_currentTarget = targets[m_targetIndex];
            }
            else
            {
                m_currentTarget = targets[m_targetIndex++];
            }
            
        }
    }
}
