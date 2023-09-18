using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Unit", menuName = "Enemy")]
public class ScriptableEnemyUnit : ScriptableObject
{
    public EnemyType enemyType;

    public EnemyStats enemyStats;

    public string enemyName;
    public GameObject enemyPrefab;
}

[Serializable]
public struct EnemyStats
{
    public int movementSpeed;
    public int health;
    public int attack;
}

[Serializable]
public enum EnemyType
{
    enemy1,
    enemy2
}