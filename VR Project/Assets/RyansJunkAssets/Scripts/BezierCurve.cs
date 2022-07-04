using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    Vector3 start;
    Vector3 controlPoint;
    Vector3 end;

    public BezierCurve(Vector3 s, Vector3 cP, Vector3 e)
    {
        start = s;
        controlPoint = cP;
        end = e;
    }
    public float FindX(float t)
    {
        float x = 0.0f;

        // c stands for the components of the equation
        float c1 = ((1 - t) * (1 - t)) * start.x;
        float c2 = 2 * (1 - t) * (t * controlPoint.x);
        float c3 = (t * t) * end.x;

        x = c1 + c2 + c3;

        return x;
    }

    public float FindY(float t)
    {
        float y = 0.0f;

        // c stands for the components of the equation
        float c1 = ((1 - t) * (1 - t)) * start.y;
        float c2 = 2 * (1 - t) * (t * controlPoint.y);
        float c3 = (t * t) * end.y;

        y = c1 + c2 + c3;

        return y;
    }
    public float FindZ(float t)
    {
        float z = 0.0f;

        float c1 = ((1 - t) * (1 - t)) * start.z;
        float c2 = 2 * (1 - t) * (t * controlPoint.z);
        float c3 = (t * t) * end.z;

        z = c1 + c2 + c3;

        return z;
    }

    public Vector3 GetStart()
    {
        return start;
    }

    public Vector3 GetControlPoint()
    {
        return controlPoint;
    }

    public Vector3 GetEnd()
    {
        return end;
    }
}
