using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class PoolData
{
    public GameObject m_PoolObject;
    public bool m_WarmUp;
    public int m_WarmupCount;
}

[DefaultExecutionOrder(-1)]
public class PoolManager : SingletonMB<PoolManager>
{
    public List<PoolData> m_PoolDatas;

    private Dictionary<GameObject, ObjectPool<GameObject>> _prefabLookup;
    private Dictionary<GameObject, ObjectPool<GameObject>> _instanceLookup; 
    
    private void Awake()
    {
        _prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();

        foreach (var poolData in m_PoolDatas)
        {
            WarmPool(poolData.m_PoolObject, poolData.m_WarmUp ? poolData.m_WarmupCount : 1);
        }
    }

    public GameObject SpawnObject(GameObject prefab)
    {
        return SpawnObject(prefab, Vector3.zero, Quaternion.identity);
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_prefabLookup.TryGetValue(prefab, out var objectPool))
            objectPool = WarmPool(prefab, 1);

        var clone = objectPool.GetItem();
        clone.transform.SetPositionAndRotation(position, rotation);
        clone.SetActive(true);



        return clone;
    }

    public T SpawnObject<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        return SpawnObject(prefab.gameObject, position, rotation).GetComponent<T>();
    }

    public void ReleaseObject(GameObject clone)
    {
        clone.SetActive(false);

        if (_instanceLookup.TryGetValue(clone, out var objectPool))
        {
            objectPool.ReleaseItem(clone);
        }
        else
            Debug.LogWarning("No pool contains the object: " + clone.name);
    }

    public void ReleaseObject(Component clone)
    {
        ReleaseObject(clone.gameObject);
    }

    public void ReleaseAll()
    {
        foreach (var clone in _instanceLookup.Keys)
            clone.SetActive(false);

        foreach (var objectPool in _prefabLookup.Values)
            objectPool.ReleaseAll();
    }

    private ObjectPool<GameObject> WarmPool(GameObject prefab, int size)
    {
        Assert.IsFalse(
            _prefabLookup.ContainsKey(prefab),
            $"Pool for prefab {prefab.name} has already been created");

        var objectPool = new ObjectPool<GameObject>(objectPool => InstantiatePrefab(objectPool, prefab), size);
        _prefabLookup.Add(prefab, objectPool);

        return objectPool;
    }

    private GameObject InstantiatePrefab(ObjectPool<GameObject> objectPool, GameObject prefab)
    {
        var clone = Instantiate(prefab, transform, true);
        _instanceLookup.Add(clone, objectPool);
        clone.SetActive(false);

        return clone;
    }
    
}