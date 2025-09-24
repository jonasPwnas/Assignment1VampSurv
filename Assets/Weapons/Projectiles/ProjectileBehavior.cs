using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
    public ProjectileData projectileData;
    public abstract void LaunchProjectile();
    public abstract void DisableProjectile();
}