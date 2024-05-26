using System.Collections;
using UnityEngine;

public class MoveState : State
{
    private const float pathUpdateMoveThreshold = 0.5f;
    private const float minPathUpdateTime = 0.2f;

    private Transform target;
    private Path path;

    public MoveState(Unit _unit, Transform _target) : base(_unit) { 
        target = _target;
    }

    public override void Enter() {
        if (target != null) {
            CoroutineManager.Instance.StartManagedCoroutine (UpdatePath(target));
        }
    }

    public override void Execute() {

    }

    public override void Exit() {
        // We may need some way to break the path if we want to do something else?
    }

    public void UpdateTarget(Transform _target) {
        if (_target != null) {
            target = _target;
            UpdatePath(target);
        }
    }

    private IEnumerator UpdatePath(Transform _target) {
        if (Time.timeSinceLevelLoad < 0.3f) {
            yield return new WaitForSeconds(0.3f);
        }

        PathRequestManager.RequestPath(new PathRequest(unit.position, _target.position, OnPathFound));

        float sqrMoveTherhold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = _target.position;

        while (true) {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((_target.position - targetPosOld).sqrMagnitude > sqrMoveTherhold) {
                PathRequestManager.RequestPath(new PathRequest(unit.position, _target.position, OnPathFound));
                targetPosOld = _target.position;
            }
        }
    }

    private void OnPathFound(Vector3[] _waypoints, bool _success) {
        if (_success) {
            path = new Path(_waypoints, unit.position, unit.TurnDistance, unit.StoppingDist);

            CoroutineManager coroutineManager = CoroutineManager.Instance;

            coroutineManager.StopCoroutine(FollowPath());
            coroutineManager.StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath() {
        bool followingPath = true;
        int pathIndex = 0;
        unit.transform.LookAt(path.lookPoints[0]);

        float speedpercent = 1f;

        while (followingPath) {
            Vector2 pos2D = new Vector2(unit.position.x, unit.position.z);
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
                if (pathIndex >= path.slowDownIndex && unit.StoppingDist > 0) {
                    speedpercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / unit.StoppingDist);
                    if (speedpercent < 0.01f) {
                        followingPath = false;
                    }
                }

                Quaternion targetRot = Quaternion.LookRotation(path.lookPoints[pathIndex] - unit.position);
                unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, targetRot, Time.deltaTime * unit.TurnSpeed);
                unit.transform.Translate(Vector3.forward * Time.deltaTime * unit.Speed * speedpercent, Space.Self);
            }

            yield return null;
        }
    }
}
