using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemSpecific : MonoBehaviour
{
    public PoolItemScriptableObject destructionVFX;
    public float repoolTime = 1;

    public void SpawnItem()
    {
        if(destructionVFX != null)
        {
            Pool.Manager.SpawnItem(destructionVFX, this.transform.position, this.transform.rotation, Vector3.one, repoolTime);
        }
    }
}
