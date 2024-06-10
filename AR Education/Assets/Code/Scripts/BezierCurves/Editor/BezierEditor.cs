using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierManager))]
public class BezierEditor : Editor
{
    private int previousResolution;
    private float previousOvershoot;
    private float previousStartOvershoot;
    private float previousEndOvershoot;
    private Vector3 previousOffset;

    void OnEnable()
    {
        BezierManager manager = (BezierManager)target;
        previousResolution = manager.Resolution;
        previousOvershoot = manager.Overshoot;
        previousStartOvershoot = manager.StartOvershoot;
        previousEndOvershoot = manager.EndOvershoot;
        previousOffset = manager.Offset;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BezierManager manager = (BezierManager)target;

        if (manager.Resolution != previousResolution || manager.Offset != previousOffset ||
            manager.Overshoot != previousOvershoot || manager.StartOvershoot != previousStartOvershoot || manager.EndOvershoot != previousEndOvershoot)
        {
            manager.RefreshAll();
            previousResolution = manager.Resolution;
            previousOvershoot = manager.Overshoot;
            previousStartOvershoot = manager.StartOvershoot;
            previousEndOvershoot = manager.EndOvershoot;
            previousOffset = manager.Offset;
        }
    }
}