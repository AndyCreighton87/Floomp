using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    private static PathRequestManager Instance;

    private Queue<PathResult> results = new Queue<PathResult>();

    [SerializeField] private Pathfinding pathfinding;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (results.Count > 0) {
            int itemsInQueue = results.Count;
            lock(results) {
                for (int i = 0; i < itemsInQueue; i++) {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest _request) {
        ThreadStart threadStart = delegate {
            Instance.pathfinding.FindPath(_request, Instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult _result) {
        lock(results) {
            results.Enqueue(_result);
        }
    }
}

public struct PathResult {
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] _path, bool _success, Action<Vector3[], bool> _callback) {
        path = _path;
        success = _success;
        callback = _callback;
    }
}

public struct PathRequest {
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
    }
}
