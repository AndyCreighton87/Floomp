using UnityEngine;

public class Path 
{
    public readonly Vector3[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;
    public readonly int slowDownIndex;

    public Path(Vector3[] _waypoints, Vector3 _startPos, float turnDist, float _stoppingDist) {
        lookPoints = _waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 prevPoint = V3ToV2(_startPos);

        for (int i = 0; i < lookPoints.Length; i++) {
            Vector2 currentPoint = V3ToV2(lookPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - prevPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDist;
            turnBoundaries[i] = new Line(turnBoundaryPoint, prevPoint - dirToCurrentPoint * turnDist);
            prevPoint = turnBoundaryPoint;
        }

        float distFromEndPoint = 0;
        for (int i = lookPoints.Length - 1; i > 0; i--) {
            distFromEndPoint += Vector3.Distance(lookPoints[i], lookPoints[i - 1]);
            if (distFromEndPoint > _stoppingDist) {
                slowDownIndex = i;
                break;
            }
        }
    }

    private Vector2 V3ToV2(Vector3 _v3) {
        return new Vector2(_v3.x, _v3.z);
    }

    public void DrawWithGizmos() {
        Gizmos.color = Color.black;
        foreach(Vector3 p in lookPoints) {
            Gizmos.DrawCube(p + Vector3.up, Vector3.one);
        }

        Gizmos.color = Color.white;
        foreach(Line l in turnBoundaries) {
            l.DrawWithGizmos(10);
        }
    }
}
