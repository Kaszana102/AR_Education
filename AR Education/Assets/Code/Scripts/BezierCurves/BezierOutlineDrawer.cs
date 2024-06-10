using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierOutlineDrawer : MonoBehaviour
{
    public BezierDrawer bezierDrawer;

    private LineRenderer lineRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var positions = bezierDrawer.Points.Select(Point => Point.position + BezierManager.Instance.Offset + bezierDrawer.Offset).ToArray();
        
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    private void OnDrawGizmos()
    {
        var positions = bezierDrawer.Points.Select(Point => Point.position).ToArray();

        for (int i = 0; i < positions.Length - 1; i += 1) {
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
        Gizmos.DrawLine(positions[0], positions[^1]);
    }
}
