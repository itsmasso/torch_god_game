using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

enum SpawnDirections
{
    Top,
    Bottom,
    Left,
    Right
}

public abstract class EnemyWaveSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected EnemyUnitPool enemyPoolPrefab;

    [Header("Spawn Properties")]
    [SerializeField] protected float spawnInterval, maxSpawnSpeed;
    [SerializeField] protected float spawnSpeedScaling;
    [SerializeField] protected float spawnPadding;
    [SerializeField] protected bool spawnEnemies;
    [SerializeField] protected float maximumEnemySpawns;
    [SerializeField] protected float currentEnemyCount;


    [Header("Spawn Lists")]
    [SerializeField] protected List<ScriptableEnemyUnit> potentialEnemyList = new List<ScriptableEnemyUnit>();
    [SerializeField] protected List<EnemyUnitPool> wavePoolList = new List<EnemyUnitPool>();
    [SerializeField] protected List<float> weights = new List<float>();

    private SpawnDirections randomDirection;
    protected Camera cam;
    //protected float timer;



    protected virtual void Start()
    {
        EnemyBaseScript.onDeath += DecreaseEnemyCount;
        LevelManager.Instance.onNewWave += AddNewWave;
        cam = Camera.main;
        spawnEnemies = true;

        FillEnemyList();
        StartCoroutine(SpawnWaves());

        
        LevelManager.Instance.onEndOfFloor += DisableEnemySpawns;

    }

    protected Vector2 GetRandomSpawnPosition()
    {
        //In this method we can find a random spawn position of screen by finding the half camera height and width.
        //Then based on a random direction generated using enums, we add the half camera height and width + padding 
        //If the spawn manages to land off screen, then we try spawning it on the opposite side instead

        randomDirection = (SpawnDirections)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(SpawnDirections)).Length);

        float halfCameraHeight = cam.orthographicSize;
        float halfCameraWidth = halfCameraHeight * cam.aspect;

        Vector2 spawnPosition = Vector2.zero;

        switch (randomDirection)
        {
            case SpawnDirections.Top:
                if (cam.transform.position.y + halfCameraHeight + spawnPadding >= LevelManager.Instance.maxYBounds)
                    randomDirection = SpawnDirections.Bottom;
                else
                    spawnPosition = new Vector2(UnityEngine.Random.Range(cam.transform.position.x - halfCameraWidth, cam.transform.position.x + halfCameraWidth), cam.transform.position.y + halfCameraHeight + spawnPadding);
                
                break;
            case SpawnDirections.Bottom:
                if (cam.transform.position.y - halfCameraHeight - spawnPadding <= LevelManager.Instance.minYBounds)
                    randomDirection = SpawnDirections.Top;
                else
                    spawnPosition = new Vector2(UnityEngine.Random.Range(cam.transform.position.x - halfCameraWidth, cam.transform.position.x + halfCameraWidth), cam.transform.position.y - halfCameraHeight - spawnPadding);
                break;
            case SpawnDirections.Left:
                if (cam.transform.position.x - halfCameraWidth - spawnPadding <= LevelManager.Instance.minXBounds)
                    randomDirection = SpawnDirections.Right;
                else
                    spawnPosition = new Vector2(cam.transform.position.x - halfCameraWidth - spawnPadding, UnityEngine.Random.Range(cam.transform.position.y - halfCameraHeight, cam.transform.position.y + halfCameraHeight));
                break;
            case SpawnDirections.Right:
                if (cam.transform.position.x + halfCameraWidth + spawnPadding >= LevelManager.Instance.maxXBounds)
                    randomDirection = SpawnDirections.Left;
                else
                    spawnPosition = new Vector2(cam.transform.position.x + halfCameraWidth + spawnPadding, UnityEngine.Random.Range(cam.transform.position.y - halfCameraHeight, cam.transform.position.y + halfCameraHeight));
                break;
            default:
                break;
        }

        return spawnPosition;
    }

    protected void FillEnemyList()
    {
        int currentLevel = GameManager.Instance.level;

        switch (currentLevel)
        {
            case 1:
                foreach (ScriptableEnemyUnit enemy in ResourceSystem.Instance.tier1Enemies)
                {
                    potentialEnemyList.Add(enemy);
                }
                break;
            default:
                break;

        }
    }

    protected void NormalizeWeights()
    {
        float totalWeight = 0;
        
        for(int i = 0; i < weights.Count; i++)
        {
            totalWeight += weights[i];
        }

        for(int i = 0; i < weights.Count; i++)
        {
            weights[i] = weights[i] / totalWeight;
        }
    }

    protected void DecreaseEnemyCount()
    {
        currentEnemyCount--;
    }

    protected virtual void AddNewWave()
    {
        //if we add no priority, weight, new enemies wont have heavier weight and spawns will be more random. Remove if we want more random
        EnemyUnitPool newPool = Instantiate(enemyPoolPrefab, transform.position, Quaternion.identity);
        newPool.CreatePool(potentialEnemyList[UnityEngine.Random.Range(0, potentialEnemyList.Count)]);
        float priorityWeight = 0.5f;
        weights.Add(priorityWeight);
        wavePoolList.Add(newPool);
        for(int i = 0; i < wavePoolList.Count; i++)
        {
            float weight = UnityEngine.Random.Range(0, priorityWeight);
            if (wavePoolList[i] != newPool)
            {
                weights[i] = weight;
            }

        }

        NormalizeWeights();
        spawnInterval -= spawnSpeedScaling;
        if (spawnInterval < maxSpawnSpeed)
        {
            spawnInterval = maxSpawnSpeed;
        }
    }

    protected virtual void SpawnByEnemyType(EnemyUnitPool pool)
    {
        //using the enemy pool, we can retrieve an enemy and spawn it in
        //we set it stats and set its position to offscreen
        GameObject enemyUnit = pool.pool.Get();
        enemyUnit.GetComponent<EnemyBaseScript>().enemyPool = pool;
        enemyUnit.GetComponent<EnemyBaseScript>().SetEnemyStats(pool.enemyScriptable.enemyStats);
        enemyUnit.transform.position = GetRandomSpawnPosition();
        currentEnemyCount++;
    }

    protected IEnumerator SpawnWaves()
    {
        while (spawnEnemies)
        {
            yield return new WaitForSeconds(spawnInterval);
            
            //spawning enemies based on their weights
            if(currentEnemyCount <= maximumEnemySpawns)
            {
                float value = UnityEngine.Random.value;
                for (int i = 0; i < weights.Count; i++)
                {
                    if (value < weights[i])
                    {
                        SpawnByEnemyType(wavePoolList[i]);
                    }

                    value -= weights[i];
                }
            }
            
        }
    }

    protected void DisableEnemySpawns()
    {
        spawnEnemies = false;
    }


    protected virtual void Update()
    {
        //timer += Time.deltaTime;
    }

    protected virtual void OnDestroy()
    {
        LevelManager.Instance.onNewWave -= AddNewWave;
        EnemyBaseScript.onDeath -= DecreaseEnemyCount;
        LevelManager.Instance.onEndOfFloor -= DisableEnemySpawns;
    }
}
