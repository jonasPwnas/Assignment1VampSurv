using System;
using System.Collections;
using UnityEngine;

public class FoodHealthPickup : MonoBehaviour
{
    [SerializeField] private float healAmount = 25f;

    private Collider m_collider;

    IEnumerator Start()
    {
        m_collider = GetComponent<Collider>();
        yield return new WaitForSeconds(1f);
        m_collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("HEAL NAME: " + other.name);
        IHealthAndDamage health = other.GetComponent<IHealthAndDamage>();
        health?.Heal(healAmount);
        Destroy(gameObject);
    }
}
