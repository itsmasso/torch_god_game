using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//making enemy spawner inherit from abstract spawner class in case we want to mix and match adding in bosses and what not.
//add random chance for certain spawn events based on current wave or time
public class FloorOneSpawner : EnemyWaveSpawner
{
    [SerializeField] private List<ScriptableEnemyUnit> upgradedEnemyList1 = new List<ScriptableEnemyUnit>();
    [SerializeField] private float chanceToSpawnUpgradedEnemy;
    protected override void Start()
    {
        base.Start();
        
        
    }


    protected IEnumerator RandomlySpawnUpgradedList(List<ScriptableEnemyUnit> enemyList)
    {
        //This coroutine will generate a random value between 0 and 1. With that we can determine percent chance for something to happen.
        //In this case, we set the chance to spawn an upgraded enemy to our desired value and use that to determine whether random value is greater or less than that.
        //we have a chance to spawn an upgraded enemy every 1 second.
        while (spawnEnemies)
        {
            float randValue = Random.value;
            ScriptableEnemyUnit randomEnemy = enemyList[Random.Range(0, enemyList.Count)];
            if (randValue < chanceToSpawnUpgradedEnemy)
            {
                GameObject enemyUnit = Instantiate(randomEnemy.enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                enemyUnit.GetComponent<EnemyBaseScript>().SetEnemyStats(randomEnemy.enemyStats);
            }
            yield return new WaitForSeconds(1);
        }

    }

    

    protected override void Update()
    {
        base.Update();
        
        
    }

}
