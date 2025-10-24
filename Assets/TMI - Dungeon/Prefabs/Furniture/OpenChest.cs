using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class OpenChest : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject m_xpThing;
    
    private static readonly int Open = Animator.StringToHash("Open");
    private Animator m_animator;
    private float m_interactDistance = 6f;
    private bool m_isOpened = false;
    private int m_baseSpawnAMount = 25;
    private float m_maxSpawnRadius = 4f;
    private float m_minSpawnRadius = 1.5f;
    private float m_maxJumpHeight = 4f;
    private float m_minJumpHeight = 2f;

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
            SpawnLotsOFXp();
        }
    }

    private void SpawnLotsOFXp()
    {
        for (int i = 0; i < m_baseSpawnAMount; i++)
        {
            float delay = 0.1f;
            delay += i / 100f;
            StartCoroutine(MoveThenEnable(delay, 1.2f));
        }
    }

    IEnumerator MoveThenEnable(float initialDelay, float duration)
    {
        yield return new WaitForSeconds(initialDelay);
        XpPickup xpDrop = Instantiate(m_xpThing, transform.position, m_xpThing.transform.rotation).GetComponent<XpPickup>();
        xpDrop.SpawnedFromChest();
        Vector3 jumpTarget = transform.position + Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up)
            * transform.forward * Random.Range(m_minSpawnRadius, m_maxSpawnRadius);
        xpDrop.transform.DOJump(jumpTarget, Random.Range(m_minJumpHeight, m_maxJumpHeight), 1, duration);
        
        yield return new WaitForSeconds(duration + 0.7f);
        xpDrop.EnablePickup();
    }
    
}
