using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "Scriptable Objects/Projectile data")]
public class ProjectileData : ScriptableObject
{
    public enum SpawnLocation{OnTheGround, MiddleOfPlayer, AbovePlayer, SkyHigh}
    public enum ProjectileTargeting {None, ClosestEnemy}
    public enum ProjectileMovement {Forward, Downward}
    
    [Tooltip("Time between spawn and movement starts, in seconds")]
    [SerializeField] public float launchTime = 1f;
    
    [Tooltip("Radius of the projectile")]
    [SerializeField] public float projectileRadius = 0.5f;
    
    [Tooltip("Damage done on impact")]
    [SerializeField] public float projectileDamage = 1f;
    
    [Tooltip("Radius of the impact made when colliding with something")]
    [SerializeField] public float impactRadius = 1f;
    
    [Tooltip("Where is the projectile spawned")]
    [SerializeField] public SpawnLocation spawnLocation = SpawnLocation.MiddleOfPlayer;
    
    [Tooltip("Does the projectile target the closest enemy?")]
    [SerializeField] public ProjectileTargeting projectileTargeting = ProjectileTargeting.None;
    
    [Tooltip("In what direction does the projectile travel?")]
    [SerializeField] public ProjectileMovement projectileMovement = ProjectileMovement.Forward;
    
    //Probably rethink this, want the WHOLE weapon to be configurable and useable
}
