using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBase : PowerUp
{
    public PowerUpType powerUpType;
    public enum PowerUpType
    {
        Armor,
        Shield,
        Weapon,
    }

    public Renderer primaryRenderer;
    
    public bool IsOnScreen()
    {
        if(primaryRenderer.isVisible)
        {
            return true;
        }
        return false;
    }
    
    public bool canActivate
    {
        get
        {
            if(locked||!attached)
            {
                return false;
            }

            return timeSinceLastActivation >= activationDelay;
        }
    }
    public float activationDelay = .25f;
    public float timeSinceLastActivation;

    void OnTriggerEnter(Collider collision)
    {
        if(IsOnScreen())
        {
            if(locked)
            {
                return;
            }

            if(!attached)
            {
                //We are attempting to add the powerup now.
                GameObject g = collision.gameObject;

                PowerUp pu = g.GetComponent<PowerUp>();
                if(pu != null)
                {
                    this.parent = pu;

                    pu.AddPowerup(this);
                    
                    attached = true;
                    
                    this.transform.parent = pu.transform;
                    OnAttached();


                    PowerUpCollector puc = collision.gameObject.GetComponent<PowerUpCollector>();
                    if(puc != null)
                    {
                        if(puc.IsPlayer)
                        {
                            this.gameObject.layer = puc.gameObject.layer;
                            puc.AddPowerupToAllChildren(this);
                        }
                    }
                    else
                    {
                        while(pu.parent != null)
                        {
                            pu = pu.parent;

                            if(pu.parent == null)
                            {
                                this.gameObject.layer = pu.gameObject.layer;
                                pu.GetComponent<PowerUpCollector>().AddPowerupToAllChildren(this);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public virtual void Update()
    {
        if(timeSinceLastActivation<activationDelay)
        {
            timeSinceLastActivation += Time.deltaTime;
        }
    }

    public virtual void ActivatePowerUp()
    {
       
    }

    public virtual void OnAttached()
    {
        if(myRigidBody!=null)
        {
            myRigidBody.isKinematic = true;
        }
    }

    private void OnDrawGizmos()
    {
        if(!ShowPowerUpGizmos)
            return;

        if(parent!=null)
        {
            Gizmos.DrawLine(this.transform.position, parent.transform.position);
        }
    }
}