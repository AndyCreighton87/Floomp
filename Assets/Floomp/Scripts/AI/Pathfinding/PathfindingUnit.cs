using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUnit : MonoBehaviour
{
    private const float pathUpdateMoveThreshold = 0.5f;
    private const float minPathUpdateTime = 0.2f;

    [Header("Pathfinding Variables")]
    [SerializeField] private  float speed = 15f;
    [SerializeField] private float turnDistance = 5f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float stoppingDist = 10f;

    private Path path;

    public virtual void Init(Transform _target) {
        if (_target != null) {
            UpdatePath(_target);
        }
    }

    private void OnPathFound(Vector3[] _waypoints, bool _success) {
        if (_success) {
            path = new Path(_waypoints, transform.position, turnDistance, stoppingDist);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator UpdatePath(Transform _target) {
        if (Time.timeSinceLevelLoad < 0.3f) {
            yield return new WaitForSeconds(0.3f);
        }

        PathRequestManager.RequestPath(new PathRequest(transform.position, _target.position, OnPathFound));

        float sqrMoveTherhold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = _target.position;

        while (true) {
            yield return new WaitForSeconds(minPathUpdateTime);

            if((_target.position - targetPosOld).sqrMagnitude > sqrMoveTherhold) {
                PathRequestManager.RequestPath(new PathRequest(transform.position, _target.position, OnPathFound));
                targetPosOld = _target.position;
            }
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

            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {
            path.DrawWithGizmos();
        }
    }
}
