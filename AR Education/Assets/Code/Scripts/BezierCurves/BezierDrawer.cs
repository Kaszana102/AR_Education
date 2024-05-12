using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierDrawer : MonoBehaviour
{
    public Transform[] Points;

    [Range(2, 200)]
    public int Resolution;

    public Vector3 Offset;

    private LineRenderer lineRenderer;
    private Vector3[] previousPositions;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        previousPositions = Enumerable.Repeat(Vector3.zero, Points.Length).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        var positions = Points.Select(Point => Point.position + Offset).ToArray();
        
        if (!positions.SequenceEqual(previousPositions)) {
            var linePoints = BezierUtility.GeneratePoints(positions, Resolution);
            lineRenderer.positionCount = Resolution;
            lineRenderer.SetPositions(linePoints);
        }

        previousPositions = positions;
    }
}
