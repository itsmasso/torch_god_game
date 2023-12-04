using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelEventTypes
{
    GiantSkull,

}
//making enemy spawner inherit from abstract spawner class in case we want to mix and match adding in bosses and what not.
//add random chance for certain spawn events based on current wave or time
public class EnemyMainSpawner : EnemyBaseSpawner
{
    [Header("Upgraded Enemy Properties")]
    [SerializeField] private float enemyTier;
    [SerializeField] private List<ScriptableEnemyUnit> nextTierEnemyList = new List<ScriptableEnemyUnit>();
    [SerializeField] private float upgradedSpawnInterval = 3f;
    [SerializeField] private float chanceIncreaseAmount = 20;
    [SerializeField] private float currentChance;

    [Header("Reaper Spawn Properties")]
    [SerializeField] private ScriptableEnemyUnit reaperScriptableObject;

    [Header("Healing Unit Properties")]
    [SerializeField] private ScriptableEnemyUnit healingScriptableUnit;
    [SerializeField] private float chanceToSpawnHealingUnit;
    [SerializeField] private float healingUnitSpawnCheckInterval;
    private float healingUnitTimer;

    [Header("Event Properties")]
    [SerializeField] private GameObject giantSkullPrefab;
    private float eventTimer;
    [SerializeField] private float eventSpawnCheckInterval = 15f;
    [SerializeField] private float chanceToSpawnEvent = 30f;

    protected override void Start()
    {
        base.Start();

        switch (enemyTier)
        {
            case 1:
                foreach (ScriptableEnemyUnit enemy in ResourceSystem.Instance.tier1Enemies)
                {
                    nextTierEnemyList.Add(enemy);
                }
                break;
            case 2:
                foreach (ScriptableEnemyUnit enemy in ResourceSystem.Instance.tier2Enemies)
                {
                    nextTierEnemyList.Add(enemy);
                }
                break;
            default:
                break;
        }

        int currentFloor = LevelManager.Instance.saveData.playerData.currentFloor;
        switch (currentFloor)
        {
            case 3:
                currentChance += chanceIncreaseAmount;
                StartCoroutine(RandomlySpawnNextTier(nextTierEnemyList));               
                break;
            case 4:
                currentChance += chanceIncreaseAmount * 2;
                StartCoroutine(RandomlySpawnNextTier(nextTierEnemyList));
                break;
            case 5:
                currentChance += chanceIncreaseAmount * 3;
                StartCoroutine(RandomlySpawnNextTier(nextTierEnemyList));
                GameObject reaper = Instantiate(reaperScriptableObject.enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                reaper.GetComponent<EnemyBaseScript>().SetEnemyStats(reaperScriptableObject.enemyStats);
                break;
            default:
                break;
        }

    }

    protected IEnumerator RandomlySpawnNextTier(List<ScriptableEnemyUnit> enemyList)
    {
        //This coroutine will generate a random value between 0 and 1. With that we can determine percent chance for something to happen.
        //In this case, we set the chance to spawn an upgraded enemy to our desired value and use that to determine whether random value is greater or less than that.
        //we have a chance to spawn an upgraded enemy every 1 second.
        while (spawnEnemies)
        {
            float randValue = Random.value;
            ScriptableEnemyUnit randomEnemy = enemyList[Random.Range(0, enemyList.Count)];
            if (randValue < currentChance)
            {
                GameObject enemyUnit = Instantiate(randomEnemy.enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                enemyUnit.GetComponent<EnemyBaseScript>().SetEnemyStats(randomEnemy.enemyStats);
                currentEnemyCount++;
            }
            yield return new WaitForSeconds(upgradedSpawnInterval);
        }

    }  

    protected void SpawnHealingUnit()
    {
        healingUnitTimer += Time.deltaTime;
        if (healingUnitTimer >= healingUnitSpawnCheckInterval)
        {
            float randValue = Random.value;
            if (randValue < chanceToSpawnHealingUnit / 100)
            {
                GameObject newHealingUnit = Instantiate(healingScriptableUnit.enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                newHealingUnit.GetComponent<HealingUnitScript>().SetEnemyStats(healingScriptableUnit.enemyStats);
            }
            healingUnitTimer = 0;
        }
    }

    protected void TriggerEnemyEvent()
    {
        eventTimer += Time.deltaTime;
        if (eventTimer >= eventSpawnCheckInterval)
        {
            float randValue = Random.value;
            if (randValue < chanceToSpawnEvent / 100)
            {
                LevelEventTypes chosenEvent = (LevelEventTypes)Random.Range(0, System.Enum.GetValues(typeof(LevelEventTypes)).Length);
                switch (chosenEvent)
                {
                    case LevelEventTypes.GiantSkull:
                        SpawnGiantSkull();
                        break;
                    default:
                        break;
                }
            }
            eventTimer = 0;
        }
    }

    protected void SpawnGiantSkull()
    {
        //skeleton noise
        //shake screen animation
        GameObject giantSkull = Instantiate(giantSkullPrefab, GetRandomSpawnPosition(), Quaternion.identity);
    }


    protected override void Update()
    {
        base.Update();


        if (spawnEnemies)
        {
            SpawnHealingUnit();
            TriggerEnemyEvent();
        }

              
    }

}
