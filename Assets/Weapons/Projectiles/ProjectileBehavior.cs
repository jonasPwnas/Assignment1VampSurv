using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
    public ProjectileData projectileData;
    [HideInInspector] public Transform projectileTarget;
    [HideInInspector] public Collider[] colliders;
    [HideInInspector] public ProjectilePool projectilePool;
    
    public abstract void LaunchProjectile(Vector3 spawnPosition, Transform target, ProjectilePool projectilePool);
    public abstract void DisableProjectile();

    private void Start()
    {
        SetGraphics();
        print("We ran start from an abstract class HUH!!");
    }
    
    public virtual void SetGraphics()
    {
        GetComponent<MeshFilter>().mesh = projectileData.projectileMesh;
        GetComponent<MeshRenderer>().material = projectileData.projectileMaterial;
    }
}