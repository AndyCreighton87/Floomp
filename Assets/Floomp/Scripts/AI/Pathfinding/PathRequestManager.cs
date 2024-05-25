using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    private static PathRequestManager Instance;

    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();

    private PathRequest currentPathRequest;
    [SerializeField] private Pathfinding pathfinding;

    private bool isProcessingPath;

    private void Awake() {
        Instance = this;
    }

    public static void RequestPath(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(_pathStart, _pathEnd, callback);
        Instance.pathRequestQueue.Enqueue(newRequest);
        Instance.TryProcessNext();
    }

    private void TryProcessNext() {
        if (!isProcessingPath && pathRequestQueue.Count > 0) {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] _path, bool _success) {
        currentPathRequest.callback(_path, _success);
        isProcessingPath = false;
        TryProcessNext();
    }

    private struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
