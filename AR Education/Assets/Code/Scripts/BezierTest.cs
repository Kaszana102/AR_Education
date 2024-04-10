using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTest : MonoBehaviour
{
    [Range(0f, 1f)]
    public float tt;
    public Vector3[] points;

    [Range(2, 200)]
    public int resolution;
    public LineRenderer lineRenderer;

    public GameObject tester;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        var test = BezierUtility.Bezier(points, tt);
        tester.transform.position = test;
        Debug.Log(test);

        var linePoints = BezierUtility.GeneratePoints(points, resolution);
        lineRenderer.positionCount = resolution;
        lineRenderer.SetPositions(linePoints);

    }
}
