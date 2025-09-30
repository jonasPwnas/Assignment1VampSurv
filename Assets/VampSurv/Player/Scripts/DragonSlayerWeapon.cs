using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : EquipmentBase
{
    [SerializeField] private List<Collider> forwardColliders;
    [SerializeField] private Collider areaCollider;
    
    private List<GameObject> m_overlappedObjects = new List<GameObject>();

    public void EnableCollider(bool areaDamage)
    {
        if (areaDamage)
        {
            areaCollider.enabled = true;
            forwardColliders.ForEach(c => c.enabled = false);
        }
        else
        {
            areaCollider.enabled = false;
            forwardColliders.ForEach(c => c.enabled = true);       
        }
    }
    
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
        m_overlappedObjects.Clear();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        m_overlappedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        m_overlappedObjects.Remove(other.gameObject);
    }

    public override void LevelEquipmentUp()
    {
        base.LevelEquipmentUp();
    }
}
