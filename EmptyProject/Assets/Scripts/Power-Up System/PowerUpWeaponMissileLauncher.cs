using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpWeaponMissileLauncher : PowerUpBase
{
    //The more complex powerup

    //Searches for targets and pivots to look at them

    //it releases a missile that will then track the target
    public Transform barrel;


    public Transform endBarrel;
    public PoolItemScriptableObject projectile;
    PoolItem projectilePI;

    [Header("Target Finding Settings")]
    public LayerMask sightMask;
    public float sightRadius = 5f;
    public float checkFrequency = .25f;
    [HideInInspector]
    public float timeSinceLastCheck;

    public List<Transform> possibleTargets = new List<Transform>();
    public Transform target;

    public float rotationSpeed = 2;

    public override void ActivatePowerUp()
    {
        base.ActivatePowerUp();

        projectilePI = Pool.Manager.SpawnItem(projectile, endBarrel.position, endBarrel.rotation, Vector3.one, 5f);
        projectilePI.gameObject.layer = gameObject.layer;
    }

    public override void Update()
    {
        base.Update();

        if(attached)
        {
            //look at target
            CheckSurroundings();
            CheckFindings();
            LookAtTarget();
        }
    }
    
    void CheckSurroundings()
    {
        timeSinceLastCheck += Time.deltaTime;
        if(timeSinceLastCheck >= checkFrequency)
        {
            possibleTargets = new List<Transform>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, sightRadius, sightMask);

            if(colliders.Length > 0)
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].transform != this.transform)
                    {
                        possibleTargets.Add(colliders[i].transform);
                    }
                }
            }

            timeSinceLastCheck = 0;
        }
    }

    void CheckFindings()
    {
        if(possibleTargets.Count > 0)
        {
            target = possibleTargets[0];
        }
    }
    
    void LookAtTarget()
    {
        if(target == null)
            return;
        Quaternion newRotation = Quaternion.LookRotation(barrel.position - target.position, Vector3.forward);
        newRotation.Set(0, 0, newRotation.z, newRotation.w);
        barrel.rotation = Quaternion.Slerp(barrel.rotation, newRotation, Time.deltaTime * rotationSpeed);

    }

    public static float AngleDir(Transform A, Transform B)
    {
        Vector3 relativePoint = A.InverseTransformPoint(B.position);
        if(relativePoint.x < 0.0)
        {
            return -1;
        }
        else if(relativePoint.x > 0.0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void OnDrawGizmos()
    {
        if(!ShowPowerUpGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}