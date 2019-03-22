using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : PowerUpBase
{
    //This is the sphere around the powerup that acts as the shield.

    //If it takes damage, it will feed that damage to it's powerup.

    //Depending on the health, it will have a differne opacity on the shield texture

    public GameObject forcefield;


    public override void Update()
    {
        if(attached)
        {
            if(!forcefield.activeInHierarchy)
            {
                forcefield.SetActive(true);
            }
        }
        else
        {
            if(forcefield.activeInHierarchy)
            {
                forcefield.SetActive(false);
            }
        }
    }
}