using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using TMPro;

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

    [SerializeField] private int MaxScore;

    [SerializeField] private GameObject CongratUI;

    [SerializeField] private int RandomSeed;


    [SerializeField] private GameObject[] Patterns;

    
    public BezierDrawer startPattern;


    private GameObject pattern;
    private Transform[] patternTrackers;  // reference trackers of the pattern
    private List<Transform> trackers;  // trackers checked against patternTrackers

    private bool win = false;
    private float startTime;
    private int curveNumber;

    

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
        Random.seed = RandomSeed;
        curveNumber = 1;

        var allTrackers = startPattern.Points;

        pattern = Instantiate(Patterns[0], new Vector3(9999, 9999, 9999), Quaternion.identity);  // todo
        
        patternTrackers = pattern.transform.GetComponentsInChildren<BezierDrawer>()[0].Points;

        trackers = new List<Transform>();
        for (int i = 0; i < patternTrackers.Length; i += 1) {
            trackers.Add(allTrackers[i]);
        }

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!win && CheckCompletion()) {
            win = true;

            float elapsedTime = Time.time - startTime;
            int score = (int)(MaxScore - elapsedTime);

            ScoreManager.AddScore(
                new Score("Curve " + curveNumber, score),
                Game.BEZIER
            );

            CongratUI.SetActive(true);
            CongratUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "GRATULACJE\nWynik: " + score;
        }

        if (win){
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

    public void StartNewLevel()
    {
        win = false;
        Overshoot = 0;
        startTime = Time.time;
        curveNumber += 1;

        GenerateNewPattern();
        RefreshAll();
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
        }
    }
}


