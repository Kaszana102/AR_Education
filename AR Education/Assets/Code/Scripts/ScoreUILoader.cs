using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUILoader : MonoBehaviour
{

    [SerializeField]
    Game gameToLoadScores = Game.LATTICE;
    [SerializeField]
    GameObject ScorePrefab;

    void Start()
    {
        var scores = ScoreManager.GetGameScores(gameToLoadScores);
    }

}
