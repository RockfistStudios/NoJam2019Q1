using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    public float lerpSpeed = 2;
    float distToTarget;
    public Transform Target;
    public bool follow = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.Instance.isPlaying)
        {
            if(Target!=null)
            {
                distToTarget = Vector3.Distance(transform.position, Target.position);

                transform.position = Vector3.Lerp(transform.position, Target.position, lerpSpeed * distToTarget * Time.fixedDeltaTime);
            }
        }
    }
}