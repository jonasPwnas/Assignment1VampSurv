using System;
using System.Collections.Generic;
using UnityEngine;
using static EquipmentLevelUp.EquipmentLevelUpStructs;

public class DragonSlayerWeapon : EquipmentBase
{
    [SerializeField] private List<Collider> forwardColliders;
    [SerializeField] private SphereCollider areaCollider;
    [SerializeField] private EquipmentLevelUpData levelUpData;
    [SerializeField] private Transform swordTransform;
    [SerializeField] private Transform forwardColliderTransform;
    [SerializeField] private float baseDamage = 1.1f;
    [SerializeField] private int startLevel;
    
    private List<GameObject> m_overlappedObjects = new List<GameObject>();

    private void Start()
    {
        LevelEquipmentUp(startLevel);
    }
    
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
    
    public void DealDamage(Vector3 hitDirection)
    {
        if(m_overlappedObjects.Count < 1)
            return;
        
        m_overlappedObjects.ForEach(obj =>
        {
            IHealthAndDamage healthAndDamage = obj.GetComponent<IHealthAndDamage>();
            if (healthAndDamage != null)
            {
                healthAndDamage.TakeDamage(baseDamage, hitDirection);
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

    public override void LevelEquipmentUp(int level)
    {
        MeleeWeaponLevelUpData data = (MeleeWeaponLevelUpData)levelUpData.GetLevelUpData(level);
        areaCollider.radius += data.rangeIncrease * 0.35f;
        forwardColliderTransform.localScale += new Vector3(data.rangeIncrease * 0.2f, 0f, data.rangeIncrease * 0.5f);
        swordTransform.localScale += new Vector3(data.rangeIncrease * 44f, 44f, data.rangeIncrease * 66f);
        baseDamage += data.damageIncrease;
    }

    [ContextMenu("DebugLevelUp")]
    private void DebugLevelUp()
    {
        startLevel++;
        LevelEquipmentUp(startLevel);
    }
}
