using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierDrawer : MonoBehaviour
{
    public Transform[] Points;

    private LineRenderer lineRenderer;
    private Vector3[] previousPositions;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        var positions = Points.Select(Point => Point.position + BezierManager.Instance.Offset).ToArray();
        
        if (!positions.SequenceEqual(previousPositions)) {
            var linePoints = BezierUtility.GeneratePoints(positions, BezierManager.Instance.Resolution,
                                                          BezierManager.Instance.StartOvershoot, BezierManager.Instance.EndOvershoot, BezierManager.Instance.Overshoot);
            lineRenderer.positionCount = BezierManager.Instance.Resolution;
            lineRenderer.SetPositions(linePoints);
        }

        previousPositions = positions;
    }

    private void OnDrawGizmos()
    {
        var positions = Points.Select(Point => Point.position).ToArray();
        var linePoints = BezierUtility.GeneratePoints(positions, 200);  // default resolution of 200, as the DrawManager.Instance doesn't exist in editor

        for (int i = 0; i < linePoints.Length - 1; i += 1) {
            Gizmos.DrawLine(linePoints[i], linePoints[i + 1]);
        }
    }

    public void Refresh()
    {
        previousPositions = Enumerable.Repeat(Vector3.zero, Points.Length).ToArray();        
    }
}
