using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

enum SpawnDirections
{
    Top,
    Bottom,
    Left,
    Right
}


public abstract class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField]
    protected ScriptableEnemyUnit wave1Enemy, wave2Enemy, wave3Enemy, wave4Enemy, wave5Enemy;

    protected int currentWave1EnemyCount, currentWave2EnemyCount, currentWave3EnemyCount, currentWave4EnemyCount, currentWave5EnemyCount;

    [SerializeField]
    protected float wave1SpawnInterval, wave2SpawnInterval, wave3SpawnInterval, wave4SpawnInterval, wave5SpawnInterval;

    [SerializeField]
    protected int maxWave1EnemyCount, maxWave2EnemyCount, maxWave3EnemyCount, maxWave4EnemyCount, maxWave5EnemyCount;

    protected bool wave1Spawned, wave2Spawned, wave3Spawned, wave4Spawned, wave5Spawned = false;

    [SerializeField]
    protected EnemyUnitPool wave1Pool, wave2Pool, wave3Pool, wave4Pool, wave5Pool;

    private SpawnDirections randomDirection;
    protected Camera cam;
    //protected float timer;

    [SerializeField]
    protected float spawnPadding;

    [SerializeField]
    protected bool spawnEnemies;

    protected virtual void Start()
    {
        cam = Camera.main;
        spawnEnemies = true;
        LevelManager.Instance.onWave1 += SpawnWave1Enemies;
        LevelManager.Instance.onWave2 += SpawnWave2Enemies;
        LevelManager.Instance.onWave3 += SpawnWave3Enemies;
        LevelManager.Instance.onWave4 += SpawnWave4Enemies;
        LevelManager.Instance.onWave5 += SpawnWave5Enemies;
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

    protected virtual void SpawnByEnemyType(EnemyUnitPool pool, ScriptableEnemyUnit enemy)
    {
        //using the enemy pool, we can retrieve an enemy and spawn it in
        //we set it stats and set its position to offscreen
        GameObject enemyUnit = pool.pool.Get();
        enemyUnit.GetComponent<EnemyBaseScript>().enemyPool = pool;
        enemyUnit.GetComponent<EnemyBaseScript>().SetEnemyStats(enemy.enemyStats);
        enemyUnit.transform.position = GetRandomSpawnPosition();
    }
    protected virtual void SpawnWave1Enemies()
    {
        if (!wave1Spawned)
        {
            wave1Pool.CreatePool(wave1Enemy.enemyPrefab, maxWave1EnemyCount);
            StartCoroutine(SpawnWave1());
            wave1Spawned = true;
        }
    }
    protected IEnumerator SpawnWave1()
    {
        while (spawnEnemies)
        {
            yield return new WaitForSeconds(wave1SpawnInterval);
            SpawnByEnemyType(wave1Pool, wave1Enemy);
            currentWave1EnemyCount++;
        }
    }

    protected virtual void SpawnWave2Enemies()
    {
        if (!wave2Spawned)
        {
            wave2Pool.CreatePool(wave2Enemy.enemyPrefab, maxWave2EnemyCount);
            StartCoroutine(SpawnWave2());
            wave2Spawned = true;
        }
    }
    protected IEnumerator SpawnWave2()
    {
        while (spawnEnemies)
        {
            yield return new WaitForSeconds(wave2SpawnInterval);
            SpawnByEnemyType(wave2Pool, wave2Enemy);
            currentWave2EnemyCount++;
        }

    }
    protected virtual void SpawnWave3Enemies()
    {
        if (!wave3Spawned)
        {
            wave3Pool.CreatePool(wave3Enemy.enemyPrefab, maxWave3EnemyCount);
            StartCoroutine(SpawnWave3());
            wave3Spawned = true;
        }
    }
    protected IEnumerator SpawnWave3()
    {
        while (spawnEnemies)
        {
            yield return new WaitForSeconds(wave3SpawnInterval);
            SpawnByEnemyType(wave3Pool, wave3Enemy);
            currentWave3EnemyCount++;
        }

    }

    protected virtual void SpawnWave4Enemies()
    {
        if (!wave4Spawned)
        {
            wave4Pool.CreatePool(wave4Enemy.enemyPrefab, maxWave4EnemyCount);
            StartCoroutine(SpawnWave4());
            wave4Spawned = true;
        }
    }
    protected IEnumerator SpawnWave4()
    {
        while (spawnEnemies)
        {
            yield return new WaitForSeconds(wave4SpawnInterval);
            SpawnByEnemyType(wave4Pool, wave4Enemy);
            currentWave4EnemyCount++;
        }

    }

    protected virtual void SpawnWave5Enemies()
    {
        if (!wave5Spawned)
        {
            wave5Pool.CreatePool(wave5Enemy.enemyPrefab, maxWave5EnemyCount);
            StartCoroutine(SpawnWave5());
            wave5Spawned = true;
        }
    }
    protected IEnumerator SpawnWave5()
    {
        while (spawnEnemies)
        {
            yield return new WaitForSeconds(wave5SpawnInterval);
            SpawnByEnemyType(wave5Pool, wave5Enemy);
            currentWave5EnemyCount++;
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
        LevelManager.Instance.onWave1 -= SpawnWave1Enemies;
        LevelManager.Instance.onWave2 -= SpawnWave2Enemies;
        LevelManager.Instance.onWave3 -= SpawnWave3Enemies;
        LevelManager.Instance.onWave4 -= SpawnWave4Enemies;
        LevelManager.Instance.onWave5 -= SpawnWave5Enemies;
        LevelManager.Instance.onEndOfFloor -= DisableEnemySpawns;
    }
}
