using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public bool surveyed = false;
    public EnemySpawner enemySpawner;

    void OnTriggerEnter(Collider collision)
    {
        if(surveyed==false)
        {
            surveyed = true;
            GameManager.Instance.enemySpawners.Remove(enemySpawner);
            PlanetController.Instance.CheckList();
        }
    }
}