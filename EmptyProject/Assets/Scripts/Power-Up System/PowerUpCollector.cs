using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinksquirrel.CShake;

public class PowerUpCollector : PowerUp
{
    public bool IsPlayer = false;

    public List<PowerUpBase> allChildren = new List<PowerUpBase>();

    public void AddPowerupToAllChildren(PowerUpBase powerUpToAdd)
    {
        allChildren.Add(powerUpToAdd);

        if(IsPlayer)
        {
            GameManager.Instance.OnPowerUpsCollected();
        }
    }

    public override IEnumerator DestroyWithDelay(float delay, bool shakeCam = false)
    {
        if(IsPlayer)
        {
            GameManager.Instance.KillPlayer();
            shakeCam = true;
        }

        transform.parent = null;
        locked = true;

        yield return new WaitForSeconds(delay);

        if(HasChildren())
        {
            for(int i = 0; i < children.Count; i++)
            {
                children[i].DestroyPowerUpDelayed(shakeCam);
            }
        }

        children = new List<PowerUp>();
        allChildren = new List<PowerUpBase>();

        if(parent != null)
        {
            parent.children.Remove(this);
        }

        if(destructionVFX != null)
        {
            Pool.Manager.SpawnItem(destructionVFX, this.transform.position, this.transform.rotation, Vector3.one, 2);
        }

        if(shakeCam)
        {
            CameraShake.ShakeAll();
        }

        pi.RePool();
    }

    public void ActivatePowerups()
    {
        for(int i = 0; i < allChildren.Count; i++)
        {
            allChildren[i].ActivatePowerUp();
        }
    }
}