using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public GameObject pattern;
    private Tracker[] patternTrackers;  // reference trackers of the pattern
    private List<Tracker> trackers = new List<Tracker>();  // trackers checked against patternTrackers

    

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        var allTrackers = GameObject.FindObjectsOfType<BezierTracker>();

        pattern = Instantiate(Patterns[0], new Vector3(9999, 9999, 9999), Quaternion.identity);  // todo
        
        patternTrackers = pattern.GetComponentsInChildren<BezierTracker>();
        //for (int i = 0; i < points.Length; i += 1) {
        //    points[i].y = 0f;
        //}

        //if (allTrackers.Length < points.Length) {
        //    throw new Exception("Not enough trackers for this pattern");
        //}

        for (int i = 0; i < patternTrackers.Length; i += 1) {
            trackers.Add(allTrackers[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckCompletion()) {
            Debug.Log("HOORAYY!");
            Overshoot = Mathf.Sin(Time.time);
            RefreshAll();
        }
        else {
            Overshoot = 0;
        }

        var middlePosition = Vector3.zero;
        foreach (var tracker in trackers) {
            middlePosition += tracker.gameObject.transform.position;
        }
        middlePosition /= trackers.Count;

        var patternMiddlePosition = Vector3.zero;
        foreach (var tracker in patternTrackers) {
            patternMiddlePosition += tracker.gameObject.transform.localPosition;
        }
        patternMiddlePosition /= patternTrackers.Length;

        pattern.transform.position = middlePosition - patternMiddlePosition;
    }

    private bool CheckCompletion()
    {
        if (trackers.Count == 0) {
            return false;
        }
        bool ok = true;
        for (int i = 0; i < trackers.Count; i += 1) {
            var tracker = trackers[i];
            var patternTracker = patternTrackers[i];

            if (!(Vector3.SqrMagnitude(tracker.gameObject.transform.position - patternTracker.transform.position) < ErrorThreshold)) {
                    ok = false;
                    break;
            }
        }

        if (ok) {
            return true;
        }

        for (int i = 0; i < trackers.Count; i += 1) {
            var tracker = trackers[i];
            var patternTracker = patternTrackers[^(i+1)];

            if (!(Vector3.SqrMagnitude(tracker.gameObject.transform.position - patternTracker.transform.position) < ErrorThreshold)) {
                    return false;
            }
        }
        return true;
    }

    public void RefreshAll()
    {
        foreach (BezierDrawer drawer in GameObject.FindObjectsOfType<BezierDrawer>()) {
            drawer.Refresh();
        }
    }
}


