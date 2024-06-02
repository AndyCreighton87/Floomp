using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;

public class PathRequestManager : MonoBehaviour
{
    private static PathRequestManager Instance;

    [SerializeField] private Pathfinding pathfinding;

    private Queue<PathResult> results = new Queue<PathResult>();

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (results.Count > 0) {
            int itemsInQueue = results.Count;
            lock(results) {
                for (int i = 0; i < itemsInQueue; i++) {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success, result.requestID);
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
    public Action<Vector3[], bool, int> callback;
    public int requestID;

    public PathResult(Vector3[] _path, bool _success, Action<Vector3[], bool, int> _callback, int _requestID) {
        path = _path;
        success = _success;
        callback = _callback;
        requestID = _requestID;
    }
}

public struct PathRequest {
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool, int> callback;
    public int requestID;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool, int> _callback, int _requestID) {
        pathStart = _start;
        pathEnd = _end;
        callback = _callback;
        requestID = _requestID;
    }
}
