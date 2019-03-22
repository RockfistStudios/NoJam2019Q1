using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Circler : EnemyBase
{
    [Header("Circler Settings")]
    public float circleSpeed = 10;
    public float changeDirectionFrequency = 10;
    float timeSinceLastDirectionalChange;
    public bool isCirclingToTheRight = true;
    

    public float circleRadius = 10;
    public float circleBuffer = .5f;
    float innerCircle
    {
        get
        {
            return circleRadius - circleBuffer;
        }
    }
    float outterCircle
    {
        get
        {
            return circleRadius + circleBuffer;
        }
    }
    float distToTarget;
    bool inBufferZone
    {
        get
        {
            if(distToTarget<outterCircle && distToTarget>innerCircle)
            {
                return true;
            }
            return false;
        }
    }
    
    public override void ApprouchTarget()
    {
        if(target == null)
            return;

        timeSinceLastDirectionalChange += Time.deltaTime;
        if(timeSinceLastDirectionalChange>changeDirectionFrequency)
        {
            isCirclingToTheRight = !isCirclingToTheRight;
            timeSinceLastDirectionalChange = 0;
        }

        distToTarget = Vector3.Distance(transform.position, target.position);

        //too far
        if(distToTarget>outterCircle)
        {
            myRigidbody.velocity += transform.up * circleSpeed * Time.deltaTime;
        }
        else if(distToTarget < innerCircle) // To close
        {
            myRigidbody.velocity += transform.up * -circleSpeed * Time.deltaTime;
        }
        else if(inBufferZone)
        {
            if(isCirclingToTheRight)
            {
                myRigidbody.velocity += transform.right * circleSpeed * Time.deltaTime;
            }
            else
            {
                myRigidbody.velocity += transform.right * -circleSpeed * Time.deltaTime;
            }

        }

        if(myRigidbody.velocity.magnitude > maxSpeed)
        {
            myRigidbody.velocity += transform.up * -circleSpeed * Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        //UnityEditor.Handles.color = Color.red;
        //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, sightRadius);

        Vector3 forward = transform.TransformDirection(Vector3.up) * 2;
        Debug.DrawRay(transform.position, forward, Color.yellow);

        //if(target != null)
        //{
        //    UnityEditor.Handles.color = Color.green;
        //    UnityEditor.Handles.DrawWireDisc(target.position, Vector3.back, circleRadius);

        //    UnityEditor.Handles.color = Color.yellow;
        //    UnityEditor.Handles.DrawWireDisc(target.position, Vector3.back, innerCircle);
        //    UnityEditor.Handles.DrawWireDisc(target.position, Vector3.back, outterCircle);
        //}
        //else
        //{
        //    UnityEditor.Handles.color = Color.green;
        //    UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, circleRadius);

        //    UnityEditor.Handles.color = Color.yellow;
        //    UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, innerCircle);
        //    UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, outterCircle);
        //}
    }
}
