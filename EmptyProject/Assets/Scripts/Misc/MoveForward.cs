using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 2;

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition += transform.up * speed * Time.deltaTime;
    }
}