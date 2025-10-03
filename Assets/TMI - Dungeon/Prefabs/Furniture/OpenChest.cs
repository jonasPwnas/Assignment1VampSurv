using System;
using UnityEngine;

public class OpenChest : MonoBehaviour, IInteractable
{
    private static readonly int Open = Animator.StringToHash("Open");
    private Animator m_animator;
    private float m_interactDistance = 6f;
    private bool m_isOpened = false;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void Interact(Transform interactor)
    {
        if(!m_isOpened && Vector3.Distance(transform.position, interactor.position) < m_interactDistance)
        {
            m_isOpened = true;
            m_animator.SetTrigger(Open);
        }
    }
}
