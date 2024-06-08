using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LatticeManager : MonoBehaviour
{
    [SerializeField] private float ErrorThreshold = 0.5f;

    List<GameObject> challenges;
    List<GameObject> targetBones;
    
    List<GameObject> activeBones = null;
    GameObject activeChallenge =  null;

    [SerializeField]
    GameObject CongratUI = null;

    float startChallengeTime = 0;
    bool completedChallenge = false;


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

    void InstantiateRandomChallenge()
    {
        if (activeChallenge != null)
        {
            Destroy(activeChallenge);
        }

        var random = new System.Random();
        int index = random.Next(challenges.Count);        
        activeChallenge = Instantiate(challenges[index]);

        // create real bones list
        activeBones = new List<GameObject>();        
        RotatableOffset[] vumarks = activeChallenge.GetComponentsInChildren<RotatableOffset>();
        foreach(RotatableOffset vumark in vumarks)
        {
            // randomize offset

            //add to bones list
            activeBones.Add(vumark.obj.gameObject);
        }


        //create target bones list
        targetBones = new List<GameObject>();
        vumarks = challenges[index].GetComponentsInChildren<RotatableOffset>();
        foreach (RotatableOffset vumark in vumarks)
        {
            //add to bones list
            targetBones.Add(vumark.obj.gameObject);
        }

        startChallengeTime = Time.time;
        completedChallenge=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!completedChallenge &&(  CheckCompletion() || Time.time > 5))
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
            new Score("ma�pka", score),
            Game.LATTICE
            );
    }

    private bool CheckCompletion()
    {
        if (activeBones.Count == 0)
        {
            return false;
        }

        foreach (var bone in activeBones)
        {
            bool ok = false;
            foreach (var targetBone in targetBones)
            {
                if (Vector3.SqrMagnitude(bone.gameObject.transform.position - targetBone.transform.position) < ErrorThreshold)
                {
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                return false;
            }
        }
        return true;
    }
}
