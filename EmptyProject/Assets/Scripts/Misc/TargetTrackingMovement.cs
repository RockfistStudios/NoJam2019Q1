using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrackingMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody myRigidbody;
    public float speed = 1;
    public float turnSpeed = 1;
    public float maxSpeed = 5;

    public Transform target;

    public void Update()
    {
        LookAtTarget();
        ApprouchTarget(); 
    }

    public void LookAtTarget()
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

    public void ApprouchTarget()
    {
        if(target == null)
            return;

        if(myRigidbody.velocity.magnitude < maxSpeed)
        {
            myRigidbody.velocity += transform.up * speed * Time.deltaTime;
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
        Vector3 forward = transform.TransformDirection(Vector3.up) * 2;
        Debug.DrawRay(transform.position, forward, Color.yellow);
    }
}