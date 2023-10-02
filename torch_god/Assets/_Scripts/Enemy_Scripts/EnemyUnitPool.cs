using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnitPool : MonoBehaviour
{
    public ObjectPool<GameObject> pool { get; private set; }
    private GameObject enemyPrefab;
    [SerializeField]
    private int defaultCapacity = 20;

    [SerializeField]
    private bool collectionCheck = true;

    public void CreatePool(GameObject _enemyPrefab, int maximumPooledItems)
    {
        enemyPrefab = _enemyPrefab;
        pool = new ObjectPool<GameObject>(CreatePooledUnit, OnGetUnitFromPool, OnReleasedToPool, OnDestroyFromPool, collectionCheck, defaultCapacity, maximumPooledItems);
    }

    GameObject CreatePooledUnit()
    {
        GameObject unit = Instantiate(enemyPrefab);
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
