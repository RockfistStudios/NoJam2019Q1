using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ForceField : MonoBehaviour, IDamageable
{
    public PowerUpShield shieldParent;

    public Animator myAnim;

    public bool invencible;
    public float invencibleTime = .1f;
    public float timeSpentInvencible;

    void Update()
    {
        if(invencible)
        {
            timeSpentInvencible += Time.deltaTime;
        }
        
        if(timeSpentInvencible>=invencibleTime)
        {
            timeSpentInvencible = 0;
            invencible = false;
        }



    }

    public void RecieveDamage(int damageAmount)
    {
        if(!invencible)
        {
            myAnim.SetTrigger("Flicker");
            shieldParent.RecieveDamage(damageAmount);
            invencible = true;
        }
    }
}

