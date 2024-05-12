using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierUtility
{
    public static Vector3 Bezier(Vector3[] points, float t) {
        Vector3[] pointsCopy = new Vector3[points.Length];
        Array.Copy(points, pointsCopy, points.Length);

        return BezierInternal(pointsCopy, t, points.Length);
    }

    public static Vector3[] GeneratePoints(Vector3[] points, int resolution, float startOvershoot = 0f, float endOvershoot = 0f, float overshoot = 0f) {
        Vector3[] newPoints = new Vector3[resolution];
        for (int i = 0; i < resolution; i += 1) {
            float t = (float)i / (resolution - 1);
            newPoints[i] = Bezier(points, t * (endOvershoot + 1f + startOvershoot) - startOvershoot + overshoot);
        }
        return newPoints;
    }

    private static Vector3 BezierInternal(Vector3[] points, float t, int length) {
        if (length == 2){
            return OvershootLerp(points[0], points[1], t);
        }
        for (int i = 0; i < length - 1; i += 1){
            points[i] = OvershootLerp(points[i], points[i+1], t);
        }
        return BezierInternal(points, t, length - 1);
    }

    private static Vector3 OvershootLerp(Vector3 a, Vector3 b, float t) {
        return a + (b - a) * t;
    }
}
