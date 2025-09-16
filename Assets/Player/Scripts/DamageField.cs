using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    private List<GameObject> m_overlappedObjects = new List<GameObject>();

    public void DealDamage(float damage, Vector3 hitDirection)
    {
        if(m_overlappedObjects.Count < 1)
            return;
        
        m_overlappedObjects.ForEach(obj =>
        {
            IHealthAndDamage healthAndDamage = obj.GetComponent<IHealthAndDamage>();
            if (healthAndDamage != null)
            {
                healthAndDamage.TakeDamage(damage, hitDirection);
            }
        });
    }
    
    private void OnTriggerEnter(Collider other)
    {
        m_overlappedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        m_overlappedObjects.Remove(other.gameObject);
    }
}
