using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour , IDamageable, ISpawnable
{
    public bool ShowAsteroidGizmos = true;
    public int currentHealth = 1;
    public int maxHealth = 1;

    public PoolItemScriptableObject deathParticle;
    public PoolItemScriptableObject smallerAsteroid;
    public int numberToSpawn = 1;
    PoolItem DeathSpawnPI;
    public PoolItem pi;

    public Rigidbody myRigidBody;
    public Vector2 speedRange = new Vector2(.1f, 1f);
    Vector2 initialDirection = new Vector3();
    public float speed;
    
    public float offsetRadius;
    Vector3 offset = new Vector3();

    EnemySpawner spawnPoint;
    float distanceFromSpawnPoint;

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void Initialize(Vector3 direction)
    {
        speed = Random.Range(speedRange.x, speedRange.y);

        myRigidBody.velocity = direction * speed;
        
        myRigidBody.angularVelocity = Random.insideUnitSphere * speed * 10;
    }

    public void Initialize(Transform _spawnPoint)
    {
        spawnPoint = _spawnPoint.GetComponent<EnemySpawner>();
        speed = Random.Range(speedRange.x, speedRange.y);
        myRigidBody.velocity = Random.insideUnitCircle * speed;

        myRigidBody.angularVelocity = Random.insideUnitSphere * speed * 10;
    }

    Vector3 newDirection = new Vector3();
    void LateUpdate()
    {
        if(spawnPoint!=null)
        {
            distanceFromSpawnPoint = Vector3.Distance(this.transform.position, spawnPoint.transform.position);

            if(distanceFromSpawnPoint>=spawnPoint.area.y)
            {
                newDirection = Vector3.Lerp(Random.insideUnitCircle, this.transform.position - spawnPoint.transform.position, 0.5f);
                
                myRigidBody.velocity -= newDirection * 0.003f;
            }
        }
    }

    public void RecieveDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            //This is a terrible hack to fix a bug that I dont understand right now,.
            if(transform.position != Vector3.zero)
            {
                DeathSpawnPI = Pool.Manager.SpawnItem(deathParticle, transform.position, Quaternion.identity, Vector3.one, 2);
            }

            
            if(smallerAsteroid!=null)
            {
                for(int i = 0; i < numberToSpawn; i++)
                {
                    offset = Random.onUnitSphere * offsetRadius;
                    offset.z = 0;

                    //This is a terrible hack to fix a bug that I dont understand right now,.
                    if(transform.position != Vector3.zero)
                    {
                        DeathSpawnPI = Pool.Manager.SpawnItem(smallerAsteroid, transform.position + offset, Quaternion.identity, Vector3.one);
                        DeathSpawnPI.GetComponent<Asteroid>().Initialize(DeathSpawnPI.transform.position - this.transform.position);
                    }
                }
            }

            spawnPoint = null;

            pi.RePool();
        }
    }
    
    void OnDrawGizmos()
    {
        if(!ShowAsteroidGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, offsetRadius);
    }
}