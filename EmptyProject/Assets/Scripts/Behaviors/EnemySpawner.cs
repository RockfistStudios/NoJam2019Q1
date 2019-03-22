using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public bool ShowSpawnerGizmos = true;
    public LayerMask myMask;
    public SpawnerType spawnerType;
    public enum SpawnerType
    {
        Radial,
        Square,
    }

    public AreaSpawnType areaSpawnType;
    public enum AreaSpawnType
    {
        WithinArea,
        OnEdge,
    }

    public int maxSpawnablesInArea = 3;
    
    public Vector2 area = new Vector2(20,50);
    Vector3 randomRot = new Vector3();

    public void SpawnEnemy(PoolItemScriptableObject thingToSpawn, out PoolItem pi)
    {
        pi = Pool.Manager.SpawnItem(thingToSpawn, GetPosition(), GetRotation(), Vector3.one);
    }

    Vector2 pos2d = new Vector3();
    Vector3 pos3d = new Vector3();
    Vector3 GetPosition()
    {
        if(spawnerType == SpawnerType.Radial)
        {
            if(areaSpawnType == AreaSpawnType.WithinArea)
            {
                pos2d = Random.insideUnitCircle * area.x;
                pos3d = new Vector3(pos2d.x, pos2d.y, 0);
            }
            else
            {
                pos2d = Random.insideUnitCircle.normalized * area.x;
                pos3d = new Vector3(pos2d.x, pos2d.y, 0);
            }

            pos3d += transform.position;
        }
        else
        {
            if(areaSpawnType == AreaSpawnType.WithinArea)
            {
                pos3d = new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.x, area.x), 0) / 2;
            }
            else
            {
                switch(GetRandomSide())
                {
                    default:
                    case 0:
                        pos3d = new Vector3(area.x, Random.Range(-area.x, area.x), 0) / 2;
                        break;
                    case 1:
                        pos3d = new Vector3(-area.x, Random.Range(-area.x, area.x), 0) / 2;
                        break;
                    case 2:
                        pos3d = new Vector3(Random.Range(-area.x, area.x), area.x, 0) / 2;
                        break;
                    case 3:
                        pos3d = new Vector3(Random.Range(-area.x, area.x), -area.x, 0) / 2;
                        break;
                }
            }

            
            pos3d += transform.position;
        }

        return pos3d;
    }
    Quaternion GetRotation()
    {
        randomRot.Set(0, 0, Random.Range(0, 360));
        return Quaternion.Euler(randomRot);
    }
    int GetRandomSide()
    {
        return Random.Range(0, 4);
    }


    Collider[] hitColliders;
    public bool CanSpawnHere()
    {
        if(spawnerType == SpawnerType.Radial)
        {
            hitColliders = Physics.OverlapSphere(this.transform.position, area.y + 5, myMask);
        }
        else
        {
            hitColliders = Physics.OverlapBox(this.transform.position, new Vector3(area.y + 5, area.y + 5, 10), Quaternion.identity, myMask);
        }
        
        if(hitColliders.Length>= maxSpawnablesInArea)
        {
            return false;
        }
        else
        {
            return true;
        }
    }




    void OnDrawGizmos()
    {
        if(!ShowSpawnerGizmos)
            return;

        if(spawnerType == SpawnerType.Radial)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, area.x);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, area.y);
        }
        else
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, new Vector3(area.x, area.x,0));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(area.y, area.y, 0));
        }
    }
}