using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ObjectPool
{
    public PoolType poolType;
    public GameObject poolObject;
}

public enum PoolType { CollectParticle, BlastParticle, Collectibles }

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Manager;

    [SerializeField] ObjectPool[] poolPrefabs;

    public List<ObjectPool> objectPools = new();

    private ObjectPool pool;

    private void Awake()
    {
        Manager = this;
    }

    public GameObject GetPool(PoolType type)
    {
        if (objectPools.Exists(x => x.poolType.Equals(type)))
        {
            pool = objectPools.Find(x => x.poolType.Equals(type));

            objectPools.Remove(pool);

            pool.poolObject.SetActive(true);

            return pool.poolObject;
        }
        else
        {
            return Instantiate(poolPrefabs.ToList().Find(x => x.poolType.Equals(type)).poolObject);
        }
    }

    public void AddIntoPool(GameObject poolObj, PoolType type)
    {
        pool = new()
        {
            poolObject = poolObj,
            poolType = type
        };

        objectPools.Add(pool);

        pool.poolObject.SetActive(false);

        pool.poolObject.transform.SetParent(transform);
    }
}