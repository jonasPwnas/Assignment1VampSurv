using System;
using UnityEngine;

public class XpPickup : MonoBehaviour
{
    private bool m_pickedUp = false;
    private float m_xpValue = 1f;
    
    //Add logic to place itself on the ground on spawn/gotten from the pool
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_pickedUp = true;
            other.GetComponent<IGainXp>().GainXp(m_xpValue);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_pickedUp)
        {
            
        }
    }
}
