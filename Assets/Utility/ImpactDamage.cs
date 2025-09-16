using System;
using UnityEngine;

public class ImpactDamage : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    
    private void OnTriggerEnter(Collider other)
    {
        print("Tried to damage " + other.name + " with " + damage + " damage.");
        other.GetComponent<IHealthAndDamage>()?.TakeDamage(damage, transform.right * 5);
    }
}