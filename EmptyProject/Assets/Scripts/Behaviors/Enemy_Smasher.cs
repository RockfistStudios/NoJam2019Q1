using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Smasher : EnemyBase
{
    [Header("Smasher Settings")]
    public float ramingSpeed = 100;

    public override void AttackTarget()
    {
        if(target == null)
            return;

        timeSinceLastAttack += Time.deltaTime;
        if(timeSinceLastAttack > attackFrequency)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, sightRadius, attackMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);

                myRigidbody.velocity += transform.up * ramingSpeed * Time.deltaTime;
            }
            timeSinceLastAttack = 0;
        }
    }
}