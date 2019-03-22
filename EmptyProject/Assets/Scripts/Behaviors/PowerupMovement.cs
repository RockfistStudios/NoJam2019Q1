using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMovement : MonoBehaviour
{
    public Rigidbody myRigidBody;
    public Transform target;
    public bool rotateOnStart;
    public float accuracy = 5;
    public float maxSpeed = 1;
    public float minSpeed = .1f;
    public float speed;
    Vector3 targetPosition;

    public void Initialize(Transform _target)
    {
        target = _target;
        speed = Random.Range(minSpeed, maxSpeed);

        GetTargetPosition();
        myRigidBody.velocity = (transform.position - targetPosition) * -speed;
        if(rotateOnStart)
        {
            myRigidBody.angularVelocity = new Vector3(0,0, speed * 10);
        }
    }

    public void GetTargetPosition()
    {
        targetPosition = (Random.insideUnitCircle * accuracy);
        targetPosition += target.position;
    }

    void OnDrawGizmos()
    {
        if(target != null)
        {
            //UnityEditor.Handles.color = Color.red;
            //UnityEditor.Handles.DrawWireDisc(target.position, Vector3.back, accuracy);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(targetPosition, .5f);
        }
    }
}
