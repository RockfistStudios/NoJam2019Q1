using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpWeaponLaser : PowerUpBase
{
    public Transform endBarrel;
    public PoolItemScriptableObject projectile;
    PoolItem projectilePI;

    public override void ActivatePowerUp()
    {
        if(!canActivate)
            return;

        timeSinceLastActivation = 0;
        projectilePI = Pool.Manager.SpawnItem(projectile, endBarrel.position, endBarrel.rotation, Vector3.one, 5f);
        projectilePI.gameObject.layer = gameObject.layer;
    }
}