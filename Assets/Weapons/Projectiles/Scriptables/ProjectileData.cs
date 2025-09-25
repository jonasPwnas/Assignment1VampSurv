using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "Scriptable Objects/Projectile data")]
public class ProjectileData : ScriptableObject
{
    public enum ProjectileTargeting {None, ClosestEnemy}
    public enum ProjectileMovement {Forward, Down, DiagonalDown, DiagonalUp, Up, Custom}
    
    [Header("Projectile Settings")]
    [Tooltip("Time between spawn and movement starts, in seconds")]
    [SerializeField] public float launchTime = 1f;
    
    [Tooltip("Radius of the projectile")]
    [SerializeField] public float projectileRadius = 0.5f;
    
    [Tooltip("Damage done on impact")]
    [SerializeField] public float projectileDamage = 1f;
    
    [Tooltip("Radius of the impact made when colliding with something")]
    [SerializeField] public float impactRadius = 1f;
    
    [Tooltip("The scale of the projectile")]
    [SerializeField] public float projectileUniformScale = 1f;
    
    [Tooltip("The time it takes the projectile to impact the target, in seconds-ish")]
    [SerializeField] public float timeToImpact = 0.6f;
    
    [Tooltip("The speed of a linear, non-targeting projectile")]
    [SerializeField] public float linearProjectileSpeed = 25f;
    
    [Tooltip("The travel distance, non-targeting projectile")]
    [SerializeField] public float linearProjectileTravelDistance = 100f;
    
    [Tooltip("Does the projectile target the closest enemy?")]
    [SerializeField] public ProjectileTargeting projectileTargeting = ProjectileTargeting.None;
    
    [Tooltip("In what direction does the projectile travel?")]
    [SerializeField] public ProjectileMovement projectileMovement = ProjectileMovement.Forward;

    [Tooltip("Custom movement direction, used when custom projectile movement is chosen")]
    [SerializeField] public Vector3 customProjectileMovementDirection = Vector3.zero;
    
    [Tooltip("What should we hit? Enemies and default probably")]
    [SerializeField] LayerMask projectileHitLayerMask;
    
    [Header("Graphics Settings")]
    [SerializeField] public Mesh projectileMesh;
    [SerializeField] public Material projectileMaterial;
    [SerializeField] public GameObject projectileTrailSystem;
    [SerializeField] public GameObject projectileImpactSystem;
}
