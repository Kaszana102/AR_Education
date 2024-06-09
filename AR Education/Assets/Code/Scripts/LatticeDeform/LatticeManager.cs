using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TMPro;
public class LatticeManager : MonoBehaviour
{
    public static LatticeManager Instance { get; private set; }

    [SerializeField] private float ErrorThreshold = 0.2f;

    List<GameObject> challenges;
    List<GameObject> targetBones;
    RotatableOffset[] vumarks;

    List<GameObject> activeBones = null;
    GameObject activeChallenge =  null;

    [SerializeField]
    GameObject CongratUI = null;

    float startChallengeTime = 0;
    bool completedChallenge = false;
    bool foundAnyVumark = false;

    Transform model = null;
    string challengeName = "";

    [SerializeField]
    TextMeshProUGUI levelNameUI;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetChallenges();
        InstantiateRandomChallenge();
    }

    void GetChallenges()
    {        
        // load from resources
        challenges = Resources.LoadAll<GameObject>("Prefabs/Lattice/Challenges").ToList();
    }

    public void InstantiateRandomChallenge()
    {
        if (activeChallenge != null)
        {
            Destroy(activeChallenge);
        }

        activeChallenge = null;
        

        startChallengeTime = Time.time;
        completedChallenge=false;
        foundAnyVumark = false;

        StartCoroutine(DelayedSpawn());
    }

    IEnumerator DelayedSpawn()
    {
        int a = 0;
        while (a < 2)
        {
            a++;
            yield return null; 
        }

        var random = new System.Random();
        int index = random.Next(challenges.Count);
        activeChallenge = Instantiate(challenges[index]);

        challengeName = challenges[index].name;
        levelNameUI.text = challengeName;

        // create real bones list
        activeBones = new List<GameObject>();
        vumarks = activeChallenge.GetComponentsInChildren<RotatableOffset>();
        System.Random rand = new System.Random();
        foreach (RotatableOffset vumark in vumarks)
        {
            // randomize offset
            float offset_x = (float)rand.NextDouble() * 1 + 0.5f;
            float offset_z = (float)rand.NextDouble() * 1 + 0.5f;

            offset_x *= rand.Next(2) == 1 ? 1 : -1;
            offset_z *= rand.Next(2) == 1 ? 1 : -1;

            float offset_y = (float)rand.NextDouble() * 2;

            vumark.offset = new Vector3(offset_x, 1, offset_z);
            vumark.minYoffsetY = 0;
            vumark.maxYoffsetY = 2;

            //add to bones list
            activeBones.Add(vumark.obj.gameObject);
        }


        //create target bones list
        targetBones = new List<GameObject>();
        var targetVumarks = challenges[index].GetComponentsInChildren<RotatableOffset>();
        foreach (RotatableOffset vumark in targetVumarks)
        {
            //add to bones list
            targetBones.Add(vumark.obj.gameObject);
        }
    }

    public void FoundVumark()
    {
        foundAnyVumark = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!completedChallenge &&  CheckCompletion()  && foundAnyVumark)
        {
            Debug.Log("HOORAYY!");
            completedChallenge = true;
            Win();
        }
    }

    void Win()
    {
        CongratUI.SetActive(true);

        float elapsedTime = Time.time - startChallengeTime;

        int score = (int)(1000 - elapsedTime);

        ScoreManager.AddScore(
            new Score(challengeName, score),
            Game.LATTICE
            );

        // turn off offseters
        foreach(var vumark in vumarks)
        {
            vumark.on = false;            
        }

        CongratUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "GRATULACJE\nWynik: " + score;

        // parent model to first vumark
        model = activeChallenge.transform.GetChild(0);
        model.SetParent(vumarks[0].transform);


        // start coroutine to set bones to default position over time                   

        StartCoroutine(MoveBonesToDeafult());
    }


    IEnumerator MoveBonesToDeafult()
    {
        float startTime = Time.time;
        float duration = 2f;

        Vector3 startPos = activeChallenge.transform.localPosition;
        Vector3 endPos = new Vector3(0, 1, 0);


        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime)/duration;

            model.localPosition = Vector3.Lerp(startPos,endPos,t);

            for (int i = 0; i < targetBones.Count(); i++)
            {
                if(Vector3.Distance(activeBones[i].transform.localPosition,
                    targetBones[i].transform.localPosition) < 0.5 * Time.deltaTime)
                {
                    activeBones[i].transform.localPosition = targetBones[i].transform.localPosition;
                }
                else
                {
                    activeBones[i].transform.localPosition += 
                        (targetBones[i].transform.localPosition
                        - activeBones[i].transform.localPosition).normalized*Time.deltaTime;
                }
                
            }

            yield return null;
        }
        model.localPosition = endPos;
        CongratUI.SetActive(false);

        for(int i=0;i<targetBones.Count();i++)
        {
            activeBones[i].transform.localPosition = targetBones[i].transform.localPosition;
        }

    }

    Vector3 AveragePos(List<GameObject> objects)
    {
        Vector3 sum = Vector3.zero;
        foreach(var gameobject in objects)
        {
            sum += gameobject.transform.position;
        }
        return sum / objects.Count();
    }

    private bool CheckCompletion()
    {
        if (activeChallenge == null)
        {
            return false;
        }

        if (activeBones.Count == 0)
        {
            return false;
        }

        //calculate averages
        Vector3 targetAvg = AveragePos(targetBones);
        Vector3 activeAvg = AveragePos(activeBones);

        for(int i=0; i < activeBones.Count; i++)
        {
            Vector3 a = activeBones[i].gameObject.transform.position - activeAvg;
            Vector3 b = targetBones[i].gameObject.transform.position - targetAvg;

            if (Vector3.SqrMagnitude(a - b) < ErrorThreshold) 
            {                    
                    
            }
            else
            {
                return false;
            }
            
        }

        return true;
    }
}
