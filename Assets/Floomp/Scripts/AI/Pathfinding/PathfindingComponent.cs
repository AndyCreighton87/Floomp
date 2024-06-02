using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingComponent : MonoBehaviour
{
    private const float pathUpdateMoveThreshold = 0.5f;
    private const float minPathUpdateTime = 0.2f;

    [SerializeField] private float speed = 15f;
    [SerializeField] private float turnDistance = 5f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float stoppingDist = 1.0f;

    private Transform target;
    private Path path;
    private Node currentNode;

    private Vector3 lastPosition;
    private float moveThresholdDistanceSquared = 0.25f;

    [HideInInspector] public Action<Node, Node> OnNodeChanged;

    private int currentPathID;

    private Coroutine pathCoroutine;

    public void MoveTo(Transform _target) {
        currentPathID++;

        CancelPath();

        target = _target;
        currentNode = GridManager.Instance.NodeFromWorldPosition(transform.position);
        lastPosition = transform.position;

        StartCoroutine(UpdatePath());
    }

    public void StopMove() {
        StopAllCoroutines();
        target = null;
        path = null;
    }

    public void UpdateTarget(Transform _target) {
        if (_target != null) {
            target = _target;
        }
    }

    private IEnumerator UpdatePath() {
        if (Time.timeSinceLevelLoad < 0.3f) {
            yield return new WaitForSeconds(0.3f);
        }

        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound, currentPathID));
    }

    private void OnPathFound(Vector3[] _waypoints, bool _success, int _requestID) {
        if (_requestID != currentPathID) {
            return;
        }

        if (_success) {
            path = new Path(_waypoints, transform.position, turnDistance, stoppingDist);

            StopCoroutine(FollowPath());
            pathCoroutine = StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath() {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1f;

        while (followingPath) {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
                if (pathIndex == path.finishLineIndex) {
                    followingPath = false;
                    break;
                }
                else {
                    pathIndex++;
                }
            }

            if (followingPath) {
                // Slow down when close to target
                if (pathIndex >= path.slowDownIndex && stoppingDist > 0) {
                    float distToTarget = path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D);
                    speedPercent = Mathf.Clamp01(distToTarget / stoppingDist);

                    if (speedPercent < 0.01f) {
                        followingPath = false;
                    }
                }

                // Rotation
                Quaternion targetRot = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
                
                // Translation
                float finalSpeed = speed * speedPercent;
                Vector3 pos = Vector3.forward * Time.deltaTime * finalSpeed;
                transform.Translate(pos, Space.Self);
            }

            CheckUpdateCurrentNode();

            yield return null;
        }
    }

    private void CheckUpdateCurrentNode() {
        Vector3 newPosition = transform.position;
        float distanceSquared = (newPosition - lastPosition).sqrMagnitude;
    
        if (distanceSquared > moveThresholdDistanceSquared) {
            Node newNode = GridManager.Instance.NodeFromWorldPosition(newPosition);
            if (newNode != currentNode) {
                OnNodeChanged?.Invoke(newNode, currentNode);
                currentNode = newNode;
            }
    
            lastPosition = newPosition;
        }
    }

    public void CancelPath() {
        if (pathCoroutine != null) {
            StopCoroutine(pathCoroutine);
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {
            path.DrawWithGizmos();
        }
    }
}
