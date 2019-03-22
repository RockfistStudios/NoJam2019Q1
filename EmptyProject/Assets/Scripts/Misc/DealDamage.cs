using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DealDamage : MonoBehaviour
{
    public int damageAmount = 1;
    public bool isDestroyedAfterDealingDamage = true;
    public PoolItem myPoolItem;
    public UnityEvent OnDamage;

    void OnTriggerEnter(Collider collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if(damageable != null)
        {
            damageable.RecieveDamage(damageAmount);
            OnDamage.Invoke();
        }

        if(isDestroyedAfterDealingDamage)
        {
            myPoolItem.RePool();
        }
    }
}
