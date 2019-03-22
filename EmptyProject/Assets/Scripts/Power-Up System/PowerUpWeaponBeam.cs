using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpWeaponBeam : PowerUpBase
{
    public Transform endBarrel;
    public Transform laser;
    public GameObject chargeParticle;
    public PoolItemScriptableObject projectile;
    PoolItem projectilePI;
    
    public override void ActivatePowerUp()
    {
        if(!canActivate)
            return;

        timeSinceLastActivation = 0;
        
        StartCoroutine(FireBeam());
    }


    IEnumerator FireBeam()
    {
        chargeParticle.SetActive(true);
        //Set all this timing based off the animation.
        yield return new WaitForSeconds(1f);

        projectilePI = Pool.Manager.SpawnItem(projectile, endBarrel.position, endBarrel.rotation, Vector3.one, 2f);
        projectilePI.gameObject.layer = gameObject.layer;

        yield return new WaitForSeconds(.1f);
        chargeParticle.SetActive(false);
    }
}