using UnityEngine;

public abstract class ProjectileBehavior : MonoBehaviour
{
    public ProjectileData projectileData;
    [HideInInspector] public Transform projectileTarget;
    [HideInInspector] public Collider[] colliders;
    [HideInInspector] public ProjectilePool projectilePool;
    [HideInInspector] public GameObject meshChild;
    [HideInInspector] public GameObject trailChild;
    [HideInInspector] public MeshRenderer renderer;
    
    public abstract void LaunchProjectile(Vector3 spawnPosition, Transform target, ProjectilePool projectilePool);
    public abstract void DisableProjectile();

    private void Start()
    {
        SetGraphics();
    }
    
    public virtual void SetGraphics()
    {
        meshChild = transform.GetChild(0).gameObject;
        meshChild.GetComponent<MeshFilter>().mesh = projectileData.projectileMesh;
        meshChild.GetComponent<MeshRenderer>().material = projectileData.projectileMaterial;
        renderer = meshChild.GetComponent<MeshRenderer>();
        if (trailChild != null)
        {
            Destroy(trailChild);
            trailChild = Instantiate(projectileData.projectileTrailSystem, transform, false);
        }
        else
        {
            trailChild = Instantiate(projectileData.projectileTrailSystem, transform, false);
        }
    }
}