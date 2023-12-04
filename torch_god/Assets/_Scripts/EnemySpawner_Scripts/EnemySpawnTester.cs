using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnTester : MonoBehaviour
{
    public GameObject player;
    public ScriptableEnemyUnit enemyToSpawn;
    [SerializeField] protected EnemyUnitPool enemyPoolPrefab;
    public bool usePooling = false;

    private void Start()
    {
        if (usePooling)
        {
            EnemyToSpawn(enemyToSpawn);
        }
        else if(!usePooling)
        {
            GameObject newEnemy = Instantiate(enemyToSpawn.enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.GetComponent<HealingUnitScript>().player = player;
            newEnemy.GetComponent<HealingUnitScript>().SetEnemyStats(enemyToSpawn.enemyStats);
        }
    }

    private void EnemyToSpawn(ScriptableEnemyUnit enemy)
    {
        EnemyUnitPool newPool = Instantiate(enemyPoolPrefab, transform.position, Quaternion.identity);
        newPool.CreatePool(enemy);
        GameObject enemyUnit = newPool.pool.Get();
        enemyUnit.GetComponent<EnemyBaseScript>().player = player;
        enemyUnit.GetComponent<EnemyBaseScript>().enemyPool = newPool;
        enemyUnit.GetComponent<EnemyBaseScript>().SetEnemyStats(newPool.enemyScriptable.enemyStats);
    }

    private void Update()
    {

    }

}
