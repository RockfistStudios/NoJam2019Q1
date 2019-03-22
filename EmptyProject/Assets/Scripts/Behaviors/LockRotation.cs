using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    public Rigidbody myRigidbody;

    public bool lockXRot;
    public bool lockYRot;
    public bool lockZRot;
	
	// Update is called once per frame
	void Update ()
    {
		if(lockXRot)
        {
            if(transform.rotation.x!=0)
            {
                transform.eulerAngles.Set(0, transform.eulerAngles.y, transform.eulerAngles.z);
                myRigidbody.angularVelocity.Set(0, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
        if(lockYRot)
        {
            if(transform.rotation.y != 0)
            {
                transform.eulerAngles.Set(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                myRigidbody.angularVelocity.Set(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            }
        }
        if(lockZRot)
        {
            if(transform.rotation.z != 0)
            {
                transform.eulerAngles.Set(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                myRigidbody.angularVelocity.Set(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
        }

    }
}
