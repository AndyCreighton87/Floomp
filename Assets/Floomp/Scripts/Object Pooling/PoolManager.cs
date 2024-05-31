using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField] private List<PoolableObject> objectPrefabs;
    [SerializeField] private int initialPoolSize = 10;

    private Dictionary<string, ObjectPool<PoolableObject>> pools = new Dictionary<string, ObjectPool<PoolableObject>>();

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        BuildPools();
    }

    private void BuildPools() {
        foreach(PoolableObject prefab in objectPrefabs) {
            GameObject poolParent = new GameObject(prefab.name + " Pool");
            poolParent.transform.parent = transform;
            var poolType = prefab.ID;
            var pool = new ObjectPool<PoolableObject>(prefab, initialPoolSize, poolParent.transform);

            //var pool = Activator.CreateInstance(typeof(ObjectPool<>).MakeGenericType(poolType), prefab, initialPoolSize, poolParent.transform) as object;
            pools.Add(poolType, pool);
        }
    }

    public PoolableObject GetObject(string _id) {
        if (pools.TryGetValue(_id, out var pool)) {
            return ((ObjectPool<PoolableObject>)pool).Get();
        }
        Debug.LogError($"No object pool found for type {_id}");
        return null;
    }

    public void ReturnObject(PoolableObject _object) {
        if (pools.TryGetValue(_object.ID, out var pool)) {
            ((ObjectPool<PoolableObject>)pool).Return(_object);
        }
    }

    //public T GetObject<T>() where T : PoolableObject {
    //    Type type = typeof(T);
    //
    //    if (pools.TryGetValue(type, out var pool)) {
    //        return ((ObjectPool<T>)pool).Get();
    //    }
    //
    //    Debug.LogError($"No object pool found for type {typeof(T)}");
    //    return null;
    //}
    //
    //public void ReturnObject<T>(T _object) where T : PoolableObject {
    //    if (pools.TryGetValue(typeof(T), out var pool)) {
    //        ((ObjectPool<T>)pool).Return(_object);
    //    }
    //}
}
