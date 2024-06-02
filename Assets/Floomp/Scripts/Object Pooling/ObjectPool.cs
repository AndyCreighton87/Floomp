using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : PoolableObject
{
    private Queue<T> objects = new Queue<T>();
    private T prefab;
    private Transform parent;

    public ObjectPool(T _prefab, int _initialSize, Transform _parent = null) {
        prefab = _prefab;
        parent = _parent;

        for(int i = 0; i < _initialSize; i++) {
            T newObj = GameObject.Instantiate(prefab, _parent);
            newObj.gameObject.SetActive(false);
            objects.Enqueue(newObj);
        }
    }

    public T Get() {
        if (objects.Count == 0) { 
            T newObj = GameObject.Instantiate(prefab, parent);
            newObj.gameObject.SetActive(false);
            objects.Enqueue(newObj);
        }

        T obj = objects.Dequeue();
        obj.gameObject.SetActive(true);
        obj.OnSpawn();

        return obj;
    }

    public void Return(T obj) {
        obj.OnDespawn();
        obj.gameObject.SetActive(false);
        objects.Enqueue(obj);
    }
}
