using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnitPool : MonoBehaviour
{
    public ObjectPool<GameObject> pool { get; private set; }
    public ScriptableEnemyUnit enemyScriptable { get; private set; }
    [SerializeField]
    private int defaultCapacity = 20;

    [SerializeField]
    private int maxPoolCount;

    public int currentPoolCount;

    [SerializeField]
    private bool collectionCheck = true;

    public void CreatePool(ScriptableEnemyUnit enemyScriptableObj)
    {
        enemyScriptable = enemyScriptableObj;
        pool = new ObjectPool<GameObject>(CreatePooledUnit, OnGetUnitFromPool, OnReleasedToPool, OnDestroyFromPool, collectionCheck, defaultCapacity, maxPoolCount);
    }

    GameObject CreatePooledUnit()
    {
        GameObject unit = Instantiate(enemyScriptable.enemyPrefab);
        unit.SetActive(false);
        unit.transform.SetParent(transform);
        return unit;
    }

    void OnGetUnitFromPool(GameObject unit)
    {
        unit.gameObject.SetActive(true);
    }

    void OnReleasedToPool(GameObject unit)
    {
        unit.gameObject.SetActive(false);
    }

    void OnDestroyFromPool(GameObject unit)
    {
        Destroy(unit.gameObject);
    }
}
