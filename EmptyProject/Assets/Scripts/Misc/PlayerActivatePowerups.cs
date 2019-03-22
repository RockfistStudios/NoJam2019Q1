using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActivatePowerups : MonoBehaviour
{
    public PowerUpCollector playerCollector;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerCollector.ActivatePowerups();
        }
    }
}
