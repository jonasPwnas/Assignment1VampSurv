using System;
using UnityEngine;

public class XpPickup : MonoBehaviour
{
    private bool m_pickedUp = false;
    private float m_xpValue = 1f;
    private SphereCollider m_collider;

    public void SpawnedFromChest()
    {
        m_collider = GetComponent<SphereCollider>();
        m_collider.enabled = false;
    }

    public void EnablePickup()
    {
        m_collider.enabled = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_pickedUp = true;
            other.GetComponent<IGainXp>().GainXp(m_xpValue);
            Destroy(gameObject);
        }
    }
}
