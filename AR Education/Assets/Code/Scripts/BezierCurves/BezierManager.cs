using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierManager : MonoBehaviour
{
    public static BezierManager Instance;

    private BezierDrawer[] drawers;

    

    [Range(2, 200)]
    public int Resolution;

    [Range(-4, 4)]
    public float Overshoot;

    [Range(-1, 1)]
    public float StartOvershoot;

    [Range(-1, 1)]
    public float EndOvershoot;

    public Vector3 Offset;

    //public BezierPattern[] Patterns;
    public float ErrorThreshold = 0.1f;

    

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
        //foreach (BezierPattern pattern in Patterns) {

        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CheckCompletion()
    {
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