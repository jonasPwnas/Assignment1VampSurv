using System;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerData.OnPlayerDeath += StopWeapon;
    }

    private void OnDisable()
    {
        PlayerData.OnPlayerDeath -= StopWeapon;
    }
    
    public virtual void StartWeapon()
    {
        //Cool base implementation
    }

    public virtual void LevelWeaponUp()
    {
        //Also a very cool base implementation
    }

    public virtual void StopWeapon()
    {
        StopAllCoroutines();
    }
}
