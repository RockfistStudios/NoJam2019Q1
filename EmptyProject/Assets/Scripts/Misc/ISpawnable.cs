using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnable 
{
    void Initialize(Transform spawnPoint);

    void Initialize(Vector3 direction);
}