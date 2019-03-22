using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public float thust=1;
    public float turnSpeed=1;
    public Rigidbody myRigid;
    public float speed;
    public float speedLimit = 10;
    public float verticalInput;

    public Animator smallthrusters;
    public Animator bigthrusters;

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");

        if(verticalInput > 0)
        {
            //forward
            myRigid.velocity += transform.forward * thust * verticalInput * Time.deltaTime;

            smallthrusters.SetBool("Moving", true);
            bigthrusters.SetBool("Moving", true);
        }
        else if(verticalInput < 0)
        {
            //Back
            myRigid.velocity += transform.forward * (thust/2) * verticalInput * Time.deltaTime;
        }
        else
        {
            smallthrusters.SetBool("Moving", false);
            bigthrusters.SetBool("Moving", false);
        }


        speed = myRigid.velocity.magnitude;

        if(speed > speedLimit)
        {
            myRigid.velocity -= transform.forward * thust * verticalInput * Time.deltaTime;
        }


        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * turnSpeed * 100, 0));
    }
}
