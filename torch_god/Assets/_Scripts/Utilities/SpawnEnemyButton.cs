using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyButton : MonoBehaviour
{
    [SerializeField]
    private EnemyType enemy;

    [SerializeField]
    private EnemyUnitPool pool;

    public void Start()
    {
      
    }
    public void SpawnEnemy()
    {
        Debug.Log("Note: This function has been hidden.");
        /*
        pool.CreatePool(ResourceSystem.Instance.GetEnemyUnit(enemy));
        GameObject enemyUnit = pool.pool.Get();
        enemyUnit.GetComponent<EnemyBaseScript>().enemyPool = pool;
        enemyUnit.GetComponent<EnemyBaseScript>().SetEnemyStats(ResourceSystem.Instance.GetEnemyUnit(enemy).enemyStats);
        enemyUnit.transform.position = Vector2.zero;
        */
    }

}
