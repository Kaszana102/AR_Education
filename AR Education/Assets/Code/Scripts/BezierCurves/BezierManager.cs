using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class BezierManager : MonoBehaviour
{
    public static BezierManager Instance;

    [Range(2, 200)]
    public int Resolution;
    [Range(-4, 4)]
    public float Overshoot;
    [Range(-1, 1)]
    public float StartOvershoot;
    [Range(-1, 1)]
    public float EndOvershoot;
    public Vector3 Offset;

    [SerializeField] private float ErrorThreshold = 0.5f;

    [SerializeField] private GameObject[] Patterns;

    private Vector3[] points;  // reference points of the pattern
    private List<Tracker> trackers = new List<Tracker>();  // trackers checked against points

    

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        var pattern = Patterns[0];  // todo
        
        points = pattern.GetComponentsInChildren<BezierTracker>().Select(tracker => tracker.gameObject.transform.position).ToArray();
        for (int i = 0; i < points.Length; i += 1) {
            points[i].y = 0f;
        }

        var allTrackers = GameObject.FindObjectsOfType<BezierTracker>();
        if (allTrackers.Length < points.Length) {
            throw new Exception("Not enough trackers detected");
        }

        for (int i = 0; i < points.Length; i += 1) {
            trackers.Add(allTrackers[i]);
        }

        Instantiate(pattern);
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCompletion()) {
            Debug.Log("HOORAYY!");
        }
                
    }

    private bool CheckCompletion()
    {
        if (trackers.Count == 0) {
            return false;
        }

        foreach (var tracker in trackers) {
            bool ok = false;
            foreach (var point in points) {
                if (Vector3.SqrMagnitude(Vector3.Scale(tracker.gameObject.transform.position, new Vector3(1f, 0f, 1f)) - point) < ErrorThreshold) {
                    ok = true;
                    break;
                }
            }
            if (!ok) {
                return false;
            }
        }
        return true;
    }

    public void RefreshAll()
    {
        if (false && EditorApplication.isPlaying) {
            Debug.Log("Called in Play Mode");
        }
        else {
            foreach (BezierDrawer drawer in GameObject.FindObjectsOfType<BezierDrawer>()) {
                drawer.Refresh();
            }
        }
    }
}


[CustomEditor(typeof(BezierManager))]
public class YourScriptEditor : Editor
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
            manager.Overshoot != previousOvershoot || manager.StartOvershoot != previousStartOvershoot || manager.EndOvershoot != previousEndOvershoot) {
            manager.RefreshAll();
            previousResolution = manager.Resolution;
            previousOvershoot = manager.Overshoot;
            previousStartOvershoot = manager.StartOvershoot;
            previousEndOvershoot = manager.EndOvershoot;
            previousOffset = manager.Offset;
        }
    }
}