using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    [SerializeField] private string id;
    
    public bool isUI = false;

    public string ID => id;

    public virtual void OnSpawn() {

    }

    public virtual void OnDespawn() {

    }
}
