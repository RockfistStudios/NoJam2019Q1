using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnItemInfo
{
    public List<WeightedItem> items;
    public List<PoolItem> activeItems;
    public Dictionary<PoolItemScriptableObject, int> itemWeights;
    public Vector2 itemSpawnRate;
    float timeToNextSpawn;
    float timeSinceLastSpawn;

    public bool CanSpawn()
    {
        if(timeSinceLastSpawn>=timeToNextSpawn)
        {
            timeToNextSpawn = Random.Range(itemSpawnRate.x, itemSpawnRate.y);
            timeSinceLastSpawn = 0;
            return true;
        }
        return false;
    }

    public void InvrementTimeSinceLastSpawn()
    {
        timeSinceLastSpawn += Time.deltaTime;
    }
}

[System.Serializable]
public struct GameSessionData
{
    public float timePlayed;
    public int powerupsCollected;
}

public class GameManager : MonoBehaviour
{
    public bool ShowManagerGizmos = true;

    public static GameManager Instance;
    public UIController uicon;
    public PowerUpCollector Player;
    List<PowerUpBase> playerPowerupsUponDeath = new List<PowerUpBase>();

    public bool isPlaying = false;

    public float RadiusFromPlayer = 20;

    public Vector2 playerArea = new Vector3(506,506);
    [HideInInspector]
    public Bounds playerAreaBounds = new Bounds();
    public bool isPlayerInBounds;
    bool isShowingWarning;
    public float timeAllowedOutsideOfBounds = 30;
    float timeSpentOutsideBounds;
    public Indicator indicatorWarning;

    public NewCameraController camController;

    [HideInInspector]
    public Bounds enemyAreaBounds = new Bounds();
    
    public GameSessionData sessionData;

    public PoolItemScriptableObject playerShip;

    public SpawnItemInfo Asteroids;
    public SpawnItemInfo Enemies;
    public SpawnItemInfo Weapons;
    public SpawnItemInfo Armors;
    public SpawnItemInfo Shield;

    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public List<EnemySpawner> asteroidSpawners = new List<EnemySpawner>();
    public List<EnemySpawner> powerUpSpawners = new List<EnemySpawner>();

    PoolItem pi;
    PoolItemScriptableObject selected;
    
    public int ActionsToLoad;
    public int ActionsCompleted = 0;

    public UIPieChart piechart;
    public Animator piechartAnim;

    private void Awake()
    {
        Instance = this;
        playerAreaBounds = new Bounds(Vector3.zero, new Vector3(playerArea.x, playerArea.y, 10));
        enemyAreaBounds = new Bounds(Vector3.zero, new Vector3(playerArea.x + RadiusFromPlayer*2, playerArea.y + RadiusFromPlayer * 2, 10));
        SetupWeightedLists();

        GetTasks();
    }
    
    public void StartGame()
    {
        isPlaying = true;


        pi = Pool.Manager.SpawnItem(playerShip,Vector3.zero, Quaternion.Euler(90,-90,90), Vector3.one);
        Player = pi.GetComponent<PowerUpCollector>();

        indicatorWarning.playerPosition = Player.transform;
        camController.Target = Player.transform;

        piechartAnim.SetBool("Show", true);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!isPlaying)
            return;

        sessionData.timePlayed += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(!isPlaying)
            return;

        CheckIfInBounds();
    }


    int armor;
    int shield;
    int weapon;
    public void OnPowerUpsCollected()
    {
        sessionData.powerupsCollected++;

        armor = 0;
        shield = 0;
        weapon = 0;

        for(int i = 0; i < Player.allChildren.Count; i++)
        {
            switch(Player.allChildren[i].powerUpType)
            {
                default:
                case PowerUpBase.PowerUpType.Armor:
                    armor++;
                    break;
                case PowerUpBase.PowerUpType.Shield:
                    shield++;
                    break;
                case PowerUpBase.PowerUpType.Weapon:
                    weapon++;
                    break;
            }
        }
        
        piechart.UpdatePieChart(armor, weapon, shield);
    }

    public void UpdateProgressBar()
    {
        uicon.UpdateProgressBar(ActionsToLoad, ActionsCompleted);
    }

    void GetTasks()
    {
        foreach(EnemySpawner spawner in enemySpawners)
        {
            if(spawner.isActiveAndEnabled)
            {
                ActionsToLoad += spawner.maxSpawnablesInArea;
            }
        }

        foreach(EnemySpawner spawner in asteroidSpawners)
        {
            if(spawner.isActiveAndEnabled)
            {
                ActionsToLoad += spawner.maxSpawnablesInArea;
            }
        }

        foreach(EnemySpawner spawner in powerUpSpawners)
        {
            if(spawner.isActiveAndEnabled)
            {
                ActionsToLoad += spawner.maxSpawnablesInArea;
            }
        }
    }

    public void StartPopulatingWorld()
    {
        StartCoroutine(PopulateWorld());
    }

    IEnumerator PopulateWorld()
    {
        foreach(EnemySpawner spawner in enemySpawners)
        {
            if(spawner.isActiveAndEnabled)
            {
                for(int i = 0; i < spawner.maxSpawnablesInArea; i++)
                {
                    SpawnEnemy(spawner);
                    ActionsCompleted++;
                }

               
                UpdateProgressBar();
                yield return new WaitForEndOfFrame();
            }
        }

        foreach(EnemySpawner spawner in asteroidSpawners)
        {
            if(spawner.isActiveAndEnabled)
            {
                for(int i = 0; i < spawner.maxSpawnablesInArea; i++)
                {
                    SpawnAsteroid(spawner);
                    ActionsCompleted++;
                }

                UpdateProgressBar();
                yield return new WaitForEndOfFrame();
            }
        }

        foreach(EnemySpawner spawner in powerUpSpawners)
        {
            if(spawner.isActiveAndEnabled)
            {
                for(int i = 0; i < spawner.maxSpawnablesInArea; i++)
                {
                    SpawnPowerUp(spawner);
                    ActionsCompleted++;
                }

                
                UpdateProgressBar();
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void SpawnAsteroid(EnemySpawner spawner)
    {
        selected = WeightedRandomizer.From(Asteroids.itemWeights).TakeOne();

        spawner.SpawnEnemy(selected, out pi);
        Asteroids.activeItems.Add(pi);
        pi.GetComponent<Asteroid>().Initialize(spawner.transform);
    }

    public void SpawnEnemy(EnemySpawner spawner)
    {
        selected = WeightedRandomizer.From(Enemies.itemWeights).TakeOne();

        spawner.SpawnEnemy(selected, out pi);
        Enemies.activeItems.Add(pi);
        pi.GetComponent<EnemyBase>().Initialize(spawner.transform);
    }

    public void SpawnPowerUp(EnemySpawner spawner)
    {
        switch(Random.Range(0,3))
        {
            default:
            case 0:
                selected = WeightedRandomizer.From(Weapons.itemWeights).TakeOne();
                break;
            case 1:
                selected = WeightedRandomizer.From(Shield.itemWeights).TakeOne();
                break;
            case 2:
                selected = WeightedRandomizer.From(Armors.itemWeights).TakeOne();
                break;
        }

        spawner.SpawnEnemy(selected, out pi);
        Enemies.activeItems.Add(pi);
        pi.GetComponent<PowerUpBase>().Initialize(spawner.transform);
    }

    void SetupWeightedLists()
    {
        Asteroids.itemWeights = new Dictionary<PoolItemScriptableObject, int>();
        Asteroids.activeItems = new List<PoolItem>();

        foreach(WeightedItem weightedItem in Asteroids.items)
        {
            Asteroids.itemWeights.Add(weightedItem.item, weightedItem.weight);
        }

        Enemies.itemWeights = new Dictionary<PoolItemScriptableObject, int>();
        Enemies.activeItems = new List<PoolItem>();

        foreach(WeightedItem weightedItem in Enemies.items)
        {
            Enemies.itemWeights.Add(weightedItem.item, weightedItem.weight);
        }

        Weapons.itemWeights = new Dictionary<PoolItemScriptableObject, int>();
        Weapons.activeItems = new List<PoolItem>();

        foreach(WeightedItem weightedItem in Weapons.items)
        {
            Weapons.itemWeights.Add(weightedItem.item, weightedItem.weight);
        }

        Armors.itemWeights = new Dictionary<PoolItemScriptableObject, int>();
        Armors.activeItems = new List<PoolItem>();

        foreach(WeightedItem weightedItem in Armors.items)
        {
            Armors.itemWeights.Add(weightedItem.item, weightedItem.weight);
        }

        Shield.itemWeights = new Dictionary<PoolItemScriptableObject, int>();
        Shield.activeItems = new List<PoolItem>();

        foreach(WeightedItem weightedItem in Shield.items)
        {
            Shield.itemWeights.Add(weightedItem.item, weightedItem.weight);
        }
    }
    
    public void CheckIfInBounds()
    {
        if(Player == null)
            return;

        isPlayerInBounds = playerAreaBounds.Contains(Player.transform.position);
        
        if(!isPlayerInBounds)
        {
            if(!isShowingWarning)
            {
                indicatorWarning.ShowWarning(true);
                isShowingWarning = true;
                piechartAnim.SetBool("Show", false);
            }
            timeSpentOutsideBounds += Time.deltaTime;
            indicatorWarning.SetTimeRemainingText(timeAllowedOutsideOfBounds - timeSpentOutsideBounds);
        }
        else
        {
            if(isShowingWarning)
            {
                indicatorWarning.ShowWarning(false);
                isShowingWarning = false;
                piechartAnim.SetBool("Show", true);
                timeSpentOutsideBounds = 0;
            }
        }

        if(timeSpentOutsideBounds>= timeAllowedOutsideOfBounds)
        {
            indicatorWarning.ShowWarning(false);
            isShowingWarning = false;
            timeSpentOutsideBounds = 0;


            Player.RecieveDamage(100);
        }
    }

    public void KillPlayer()
    {
        isPlaying = false;

        playerPowerupsUponDeath = Player.allChildren;

        StartCoroutine(WaitForShipExplosion());
    }

    IEnumerator WaitForShipExplosion()
    {
        bool waiting = true;
        int i = 0;

        while(waiting)
        {
            bool allPowerUpsDetatched = true;

            for(int o = 0; o < playerPowerupsUponDeath.Count; o++)
            {
                if(playerPowerupsUponDeath[o].attached)
                {
                    allPowerUpsDetatched = false;
                }
            }
            
            yield return new WaitForSeconds(PowerUp.destroyDelay);

            if(allPowerUpsDetatched)
                waiting = false;

            i++;
            if(i > 100)
                waiting = false;

        }

        GameOver();
    }

    public void GameOver()
    {
        isPlaying = false;
        uicon.ShowGameOverScreen(sessionData);

        piechartAnim.SetBool("Show", false);
    }

    public void OnEnemyKilled()
    {
        //sessionData.enemiesKilled++;
    }
    
    private void OnDrawGizmos()
    {
        if(!ShowManagerGizmos)
            return;

        Gizmos.color = Color.green;
        if(playerAreaBounds != null)
        {
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(playerArea.x, playerArea.y, 10));
        }
        
        Gizmos.color = Color.red;
        if(playerAreaBounds != null)
        {
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(playerArea.x + RadiusFromPlayer*2, playerArea.y + RadiusFromPlayer*2, 10));
        }
        
        if(Player!=null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Player.transform.position, RadiusFromPlayer);
        }
    }
}