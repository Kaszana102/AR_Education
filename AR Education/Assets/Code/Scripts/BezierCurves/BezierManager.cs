using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BezierManager : MonoBehaviour
{
    public static BezierManager Instance;

    [Header("Draw Settings")]
    [Range(2, 200)]
    public int Resolution;
    [Range(-4, 4)]
    public float Overshoot;
    [Range(-1, 1)]
    public float StartOvershoot;
    [Range(-1, 1)]
    public float EndOvershoot;
    public Vector3 Offset;

    [Header("Gameplay Settings")]
    [SerializeField] private float ErrorThreshold = 0.5f;
    [SerializeField] private Vector2 RandomCurveSize = new Vector2(5f, 3f);
    [SerializeField] private float RandomCurveMinPointDistance = 1f;


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

            GenerateNewPattern();
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

    private void GenerateNewPattern()
    {
        var positions = new List<Vector3>();

        for (int i = 0; i < patternTrackers.Length; i += 1) {
            Vector3 newPosition;
            bool valid = false;

            do
            {
                newPosition = new Vector3(Random.Range(-RandomCurveSize.x/2, RandomCurveSize.x/2), 0f, Random.Range(-RandomCurveSize.y/2, RandomCurveSize.y/2));
                valid = true;

                foreach (var position in positions) {
                    if (Vector3.SqrMagnitude(position - newPosition) < RandomCurveMinPointDistance) {
                        valid = false;
                        break;
                    }
                }
            } while (!valid);

            positions.Add(newPosition);
        }

        for (int i = 0; i < patternTrackers.Length; i += 1) {
            patternTrackers[i].transform.position = positions[i];
        }

        if (CheckCompletion()) {
            GenerateNewPattern();
        }
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


