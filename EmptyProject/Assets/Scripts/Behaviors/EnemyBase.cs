using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable, ISpawnable
{
    public bool ShowEnemyGizmos = true;

    public int currentHealth = 1;
    public int maxHealth = 1;

    public PoolItemScriptableObject destructionVFX;
    public PoolItem pi;
  
    [Header("Movement Settings")]
    public Rigidbody myRigidbody;
    public float speed = 1;
    public float turnSpeed = 1;
    public float maxSpeed = 5;

    [Header("Target Finding Settings")]
    public LayerMask sightMask;
    public float sightRadius = 5f;
    public float checkFrequency = .25f;
    [HideInInspector]
    public float timeSinceLastCheck;

    public List<Transform> possibleTargets = new List<Transform>();
    public Transform target;

    [Header("Attack Settings")]
    public bool canAttack = true;
    public LayerMask attackMask;
    public float attackFrequency = 1;
    [HideInInspector]
    public float timeSinceLastAttack;
    public PowerUpBase laser;


    [Header("Idle Settings")]
    public bool isIdle = true;
    public float timeToIdle = 2;
    [HideInInspector]
    public float timeSinceLastEnemyContact;
    public float randomRotationFrequency = 1.5f;
    [HideInInspector]
    public float timeSinceLastRotation;
    public float randomMovementFrequency = 1;
    [HideInInspector]
    public float timeSinceLastMovement;


    EnemySpawner spawnPoint;
    float distanceFromSpawnPoint;

    //Shouldn't Ever be called
    public void Initialize(Vector3 direction)
    {
        Debug.LogError("Why did you call this function??");
    }
    public void Initialize(Transform _target)
    {
        spawnPoint = _target.GetComponent<EnemySpawner>();
        timeSinceLastEnemyContact = timeToIdle;
        timeSinceLastMovement = randomMovementFrequency;
    }
    
    void OnEnable()
    {
        currentHealth = maxHealth;

        if(laser!=null)
        {
            laser.attached = true;
        }
    }

    private void Update()
    {
        CheckSurroundings();

        CheckFindings();

        CheckDistanceToSpawnPoint();

        LookAtTarget();

        ApprouchTarget();

        AttackTarget();

        IdleBehaivor();
    }

    void CheckDistanceToSpawnPoint()
    {
        if(spawnPoint != null)
        {
            distanceFromSpawnPoint = Vector3.Distance(this.transform.position, spawnPoint.transform.position);

            if(distanceFromSpawnPoint >= spawnPoint.area.y)
            {
                target = spawnPoint.transform;
            }
        }
    }

    public virtual void CheckSurroundings()
    {
        timeSinceLastCheck += Time.deltaTime;
        if(timeSinceLastCheck>=checkFrequency)
        {
            possibleTargets = new List<Transform>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, sightRadius, sightMask);

            if(colliders.Length>0)
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

    public virtual void CheckFindings()
    {
        if(possibleTargets.Count>0)
        {
            target = possibleTargets[0];
        }
    }

    public void RecieveDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth<=0)
        {
            //This is a terrible hack to fix a bug that I dont understand right now,.
            if(transform.position != Vector3.zero)
            {
                if(destructionVFX != null)
                {
                    Pool.Manager.SpawnItem(destructionVFX, this.transform.position, this.transform.rotation, Vector3.one, 1);
                }
            }

            GameManager.Instance.OnEnemyKilled();
            pi.RePool();
        }
    }

    public virtual void LookAtTarget()
    {
        if(target == null)
            return;
    
        //to the left
        if(AngleDir(transform, target) < 0)
        {
            Vector3 left = transform.TransformDirection(Vector3.left) * 2;
            Debug.DrawRay(transform.position, left, Color.red);

            myRigidbody.angularVelocity += Vector3.forward * turnSpeed * Time.deltaTime;
        }
        else if(AngleDir(transform, target) > 0)
        {
            Vector3 right = transform.TransformDirection(Vector3.right) * 2;
            Debug.DrawRay(transform.position, right, Color.red);

            myRigidbody.angularVelocity += Vector3.forward * -turnSpeed * Time.deltaTime;
        }
    }

    public virtual void ApprouchTarget()
    {
        if(target == null)
            return;

        if(myRigidbody.velocity.magnitude < maxSpeed)
        {
            myRigidbody.velocity += transform.up * speed * Time.deltaTime;
        }
    }

    public virtual void AttackTarget()
    {
        if(target == null)
            return;

        timeSinceLastAttack += Time.deltaTime;
        if(timeSinceLastAttack>attackFrequency)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, sightRadius, attackMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            }

            laser.ActivatePowerUp();

            timeSinceLastAttack = 0;
        }
    }

    void IdleBehaivor()
    {
        if(possibleTargets.Count == 0)
        {
            timeSinceLastEnemyContact += Time.deltaTime;
        }
        else
        {
            timeSinceLastEnemyContact = 0;
        }

        if(isIdle = timeSinceLastEnemyContact >= timeToIdle)
        {
            target = null;

            timeSinceLastMovement += Time.deltaTime;
            if(timeSinceLastMovement>=randomMovementFrequency)
            {
                myRigidbody.velocity += transform.up * speed *10 * Time.deltaTime;

                timeSinceLastMovement = 0;
            }
            
            timeSinceLastRotation += Time.deltaTime;
            if(timeSinceLastRotation>=randomRotationFrequency)
            {
                myRigidbody.angularVelocity += Vector3.forward * Random.Range(-360,360) * Time.deltaTime;

                timeSinceLastRotation = 0;
            }
        }
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
        if(!ShowEnemyGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
       
        Vector3 forward = transform.TransformDirection(Vector3.up) * 2;
        Debug.DrawRay(transform.position, forward, Color.yellow);
    }
}