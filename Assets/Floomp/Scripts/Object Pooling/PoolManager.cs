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
    [SerializeField] private Transform UIParent;

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
            bool isUI = prefab.isUI;
            GameObject poolParent = null;

            if (isUI) {
                poolParent = new GameObject(prefab.name + " Pool", typeof(RectTransform));
                poolParent.transform.parent = UIParent;
            }
            else {
                poolParent = new GameObject(prefab.name + " Pool");
                poolParent.transform.parent = transform;
            }

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
}
