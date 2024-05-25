using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct Line 
{
    private const float verticalLineGradient = 1e5f;

    private float gradient;
    private float y_intercept;

    private Vector2 pointOnLine_1;
    private Vector2 pointOnLine_2;

    private float gradientPerpendicular;

    private bool approachSide;

    public Line(Vector2 _pointOnLine, Vector2 _pointPerpendicularToLine) {
        float dx = _pointOnLine.x - _pointPerpendicularToLine.x;
        float dy = _pointOnLine.y - _pointPerpendicularToLine.y;

        if (dx ==  0) {
            gradientPerpendicular = verticalLineGradient;
        }
        else {
            gradientPerpendicular = dy / dx;
        }

        if (gradientPerpendicular == 0) {
            gradient = verticalLineGradient;
        }
        else {
            gradient = -1 / gradientPerpendicular;
        }

        y_intercept = _pointOnLine.y - gradient * _pointOnLine.x;
        pointOnLine_1 = _pointOnLine;
        pointOnLine_2 = _pointOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(_pointPerpendicularToLine);
    }

    private bool GetSide(Vector2 _p) {
        return (_p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (_p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool HasCrossedLine(Vector2 _p) {
        return GetSide(_p) != approachSide;
    }

    public float DistanceFromPoint(Vector2 _p) {
        float yInterceptPerpendicular = _p.y - gradientPerpendicular * _p.x;
        float intersectX = (yInterceptPerpendicular - y_intercept) / (gradient - gradientPerpendicular);
        float intersectY = gradient * intersectX + y_intercept;
        return Vector2.Distance(_p, new Vector2(intersectX, intersectY));
    }

    public void DrawWithGizmos(float _length) {
        Vector3 lineDir = new Vector3(1, 0, gradient).normalized;
        Vector3 lineCentre = new Vector3(pointOnLine_1.x, 0, pointOnLine_1.y) + Vector3.up;
        Gizmos.DrawLine(lineCentre - lineDir * _length / 2f, lineCentre + lineDir * _length / 2f);
    }
}
