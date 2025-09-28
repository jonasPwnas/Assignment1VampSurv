using System;
using UnityEngine;

public class DamageOnOverlap : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IHealthAndDamage>()?.TakeDamage(damage, transform.forward);
    }
}