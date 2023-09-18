using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField]
    private float enemySpawnSpeed;

    [SerializeField]
    private bool spawnEnemies;

    private int currentWave;
    
    public event Action OnWaveFive;
    private bool waveFiveTriggered;
    public float timer { get; private set; }
    void Start()
    {
        waveFiveTriggered = false;
        spawnEnemies = true;
        StartCoroutine(WaveTimer());
        StartCoroutine(SpawnEnemies());
    }


    IEnumerator WaveTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timer++;
            if(timer == 10)
            {
                currentWave = 5;
            }
        }
    }
    IEnumerator SpawnEnemies()
    {
        while (spawnEnemies)
        {
            yield return new WaitForSeconds(enemySpawnSpeed);
            EnemyManager.Instance.SpawnRandomEnemy();
        }
    }
    void Update()
    {
        if(currentWave == 5 && !waveFiveTriggered)
        {
            OnWaveFive?.Invoke();
            Debug.Log("Wave 5");
            waveFiveTriggered = true;
        }
    }
}
