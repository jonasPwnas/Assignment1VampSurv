using UnityEngine;

public interface IHealthAndDamage
{
    public void TakeDamage(float damage, Vector3 hitDirection);
    public void Heal(float healAmount);
}