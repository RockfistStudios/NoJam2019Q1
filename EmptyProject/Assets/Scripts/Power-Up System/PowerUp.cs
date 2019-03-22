using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thinksquirrel.CShake;

public class PowerUp : MonoBehaviour, IDamageable, ISpawnable
{

    public bool ShowPowerUpGizmos = true;
    public static float destroyDelay = .25f;
    public string baseLayerName = "PowerUp";

    public PoolItemScriptableObject destructionVFX;
    public PoolItem pi;

    public Rigidbody myRigidBody;
    public float speed = 0.5f;

    public PowerUp parent;
    public List<PowerUp> children = new List<PowerUp>();

    public bool locked = false;
    public bool attached = false;

    public int maxHealth = 1;
    public int currentHealth;

    public virtual void OnEnable()
    {
        currentHealth = maxHealth;
        locked = false;
        attached = false;
        parent = null;
        children = new List<PowerUp>();

        if(myRigidBody != null)
        {
            myRigidBody.isKinematic = false;
        }

        this.gameObject.layer = LayerMask.NameToLayer(baseLayerName);
    }

    public virtual void OnDisable()
    {
        currentHealth = maxHealth;
        locked = false;
        attached = false;
        parent = null;
        children = new List<PowerUp>();
        this.gameObject.layer = LayerMask.NameToLayer(baseLayerName);
    }
    
    public virtual void AddPowerup(PowerUpBase powerUpToAdd)
    {
        children.Add(powerUpToAdd);
    }

    public bool HasChildren()
    {
        if(children != null)
        {
            if(children.Count>0)
            {
                return true;
            }
        }
        return false;
    }
    
    public virtual void RecieveDamage(int damnageAmount)
    {
        currentHealth -= damnageAmount;

        if(currentHealth <= 0)
        {
            DestroyPowerupImmediately();
        }
    }

    void DestroyPowerupImmediately()
    {
        StartCoroutine(DestroyWithDelay(0));
    }

    public void DestroyPowerUpDelayed(bool shakeCam = false)
    {
        StartCoroutine(DestroyWithDelay(PowerUp.destroyDelay, shakeCam));
    }

    public virtual IEnumerator DestroyWithDelay(float delay, bool shakeCam = false)
    {
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

        if(parent != null)
        {
            parent.children.Remove(this);
        }

        if(shakeCam)
        {
            CameraShake.ShakeAll();
        }

        //This is a terrible hack to fix a bug that I dont understand right now,.
        if(transform.position != Vector3.zero)
        {
            if(destructionVFX != null)
            {
                Pool.Manager.SpawnItem(destructionVFX, this.transform.position, this.transform.rotation, Vector3.one, 1);
            }
        }

        pi.RePool();
    }

    EnemySpawner spawnPoint;
    float distanceFromSpawnPoint;

    public void Initialize(Transform _spawnPoint)
    {
        spawnPoint = _spawnPoint.GetComponent<EnemySpawner>();
        //myRigidBody.velocity = Random.insideUnitCircle * speed;

        //myRigidBody.angularVelocity = Random.insideUnitCircle * speed * 10;
    }

    public void Initialize(Vector3 direction)
    {
        //myRigidBody.velocity = direction * speed;

        //myRigidBody.angularVelocity = Random.insideUnitSphere * speed * 10;
    }
}