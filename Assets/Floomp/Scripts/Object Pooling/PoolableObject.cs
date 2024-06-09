using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    [Header("Poolable Object")]
    [SerializeField] private string id;
    
    public bool isUI = false;

    public string ID => id;

    public virtual void OnSpawn() {

    }

    public virtual void OnDespawn() {

    }
}
