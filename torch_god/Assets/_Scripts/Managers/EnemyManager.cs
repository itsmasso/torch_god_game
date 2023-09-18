using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;


enum SpawnDirections
{
    Top,
    Bottom,
    Right,
    Left
}
public class EnemyManager : Singleton<EnemyManager>
{
    //TODO: later introduce conditions to spawn certain enemies at certain times
    //Method: after a certain wave, add a new enemy type into the list of enemies to spawn

    [SerializeField]
    private EnemyWaveSpawner enemyWaveSpawner;

    [SerializeField] 
    private BoxCollider2D spawnBoundariesTop, spawnBoundariesBottom, spawnBoundariesLeft, spawnBoundariesRight;

    [SerializeField]
    private Vector2 mapBounds;

    private List<ScriptableEnemyUnit> scriptableEnemyList = new List<ScriptableEnemyUnit>();

    private void Start()
    {
        enemyWaveSpawner.OnWaveFive += AddWaveFiveEnemies;
        scriptableEnemyList.Add(ResourceSystem.Instance.GetEnemyUnit(EnemyType.enemy1));

    }
    
    public void AddWaveFiveEnemies()
    {
        scriptableEnemyList.Add(ResourceSystem.Instance.GetEnemyUnit(EnemyType.enemy2));
    }

    public void SpawnRandomEnemy()
    {
        //gets random direction based on the value of enums. (values are defaulted as ints)
        SpawnDirections randomDirection = (SpawnDirections)Random.Range(0, System.Enum.GetValues(typeof(SpawnDirections)).Length);

        Vector2 spawnBoundsSize;
        Vector2 spawnBoundsCenter;
        float randomX;
        float randomY;
        Vector2 spawnPosition = Vector2.zero;

        switch (randomDirection)
        {
            case SpawnDirections.Top:
                spawnBoundsSize = spawnBoundariesTop.size;
                spawnBoundsCenter = spawnBoundariesTop.bounds.center;

                randomX = Random.Range(-spawnBoundsSize.x / 2, spawnBoundsSize.x / 2);
                randomY = Random.Range(-spawnBoundsSize.y / 2, spawnBoundsSize.y / 2);

                spawnPosition = spawnBoundsCenter + new Vector2(randomX, randomY);

                break;
            case SpawnDirections.Bottom:
                spawnBoundsSize = spawnBoundariesBottom.size;
                spawnBoundsCenter = spawnBoundariesBottom.bounds.center;

                randomX = Random.Range(-spawnBoundsSize.x / 2, spawnBoundsSize.x / 2);
                randomY = Random.Range(-spawnBoundsSize.y / 2, spawnBoundsSize.y / 2);

                spawnPosition = spawnBoundsCenter + new Vector2(randomX, randomY);
                break;
            case SpawnDirections.Left:
                spawnBoundsSize = spawnBoundariesLeft.size;
                spawnBoundsCenter = spawnBoundariesLeft.bounds.center;

                randomX = Random.Range(-spawnBoundsSize.x / 2, spawnBoundsSize.x / 2);
                randomY = Random.Range(-spawnBoundsSize.y / 2, spawnBoundsSize.y / 2);

                spawnPosition = spawnBoundsCenter + new Vector2(randomX, randomY);
                break;
            case SpawnDirections.Right:
                spawnBoundsSize = spawnBoundariesRight.size;
                spawnBoundsCenter = spawnBoundariesRight.bounds.center;

                randomX = Random.Range(-spawnBoundsSize.x / 2, spawnBoundsSize.x / 2);
                randomY = Random.Range(-spawnBoundsSize.y / 2, spawnBoundsSize.y / 2);

                spawnPosition = spawnBoundsCenter + new Vector2(randomX, randomY);
                break;
            default:
                break;
        }

        ScriptableEnemyUnit randomEnemyScriptable = scriptableEnemyList[Random.Range(0, scriptableEnemyList.Count)];
        GameObject spawnedEnemy = Instantiate(randomEnemyScriptable.enemyPrefab, spawnPosition, Quaternion.identity);
        //disclaimer: this is calling getComponent every spawn, not sure if it will cause a performance issue or not.
        //later if needed, you can add an implementation for stat modifications here
        spawnedEnemy.GetComponent<EnemyBaseScript>().SetEnemyStats(randomEnemyScriptable.enemyStats);
    }

    private void OnDestroy()
    {
        enemyWaveSpawner.OnWaveFive -= AddWaveFiveEnemies;
    }
}
