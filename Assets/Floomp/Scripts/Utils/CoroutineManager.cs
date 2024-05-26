using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour 
{
    public static CoroutineManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void StartManagedCoroutine(IEnumerator _coroutine) {
        StartCoroutine(_coroutine);
    }

    public void StopManagedCoroutine(IEnumerator _coroutine) {
        StopCoroutine(_coroutine);
    }
}
