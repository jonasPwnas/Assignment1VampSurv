using System.Collections;
using UnityEngine;

public class LinearProjectile : ProjectileBehavior
{
    private bool m_canMove;
    
    public override void LaunchProjectile()
    {
        switch (projectileData.spawnLocation)
        {
            case ProjectileData.SpawnLocation.OnTheGround:
                //DoShit
                break;
            case ProjectileData.SpawnLocation.MiddleOfPlayer:
                //DoShit
                break;
            case ProjectileData.SpawnLocation.AbovePlayer:
                //DoShit
                break;
            case ProjectileData.SpawnLocation.SkyHigh:
                //DoShit
                break;
        }
    }

    public override void DisableProjectile()
    {
        
    }

    private void MoveProjectile()
    {
        
    }
    
    IEnumerator WaitThenFire()
    {
        yield return new WaitForSeconds(projectileData.launchTime);
        m_canMove = true;
    }
}