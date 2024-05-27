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
    [SerializeField] private float stoppingDist = 10f;

    private Transform target;
    private Path path;
    private Node currentNode;

    private Vector3 lastPosition;
    private float moveThresholdDistanceSquared = 0.25f;

    [HideInInspector] public Action<Node, Node> OnNodeChanged;

    public void MoveTo(Transform _target) {
        target = _target;
        currentNode = GridManager.Instance.NodeFromWorldPosition(transform.position);
        lastPosition = transform.position;

        StartCoroutine(UpdatePath());
    }

    public void StopMove() {
        StopAllCoroutines();
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

        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveTherhold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true) {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveTherhold) {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    private void OnPathFound(Vector3[] _waypoints, bool _success) {
        if (_success) {
            path = new Path(_waypoints, transform.position, turnDistance, stoppingDist);

            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath() {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        float speedpercent = 1f;

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
                if (pathIndex >= path.slowDownIndex && stoppingDist > 0) {
                    speedpercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDist);
                    if (speedpercent < 0.01f) {
                        followingPath = false;
                    }
                }

                Quaternion targetRot = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedpercent, Space.Self);
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
}
