using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUILoader : MonoBehaviour
{

    [SerializeField]
    Game gameToLoadScores = Game.LATTICE;
    [SerializeField]
    GameObject ScorePrefab;

    void Start()
    {
        var scores = ScoreManager.GetGameScores(gameToLoadScores);

        foreach(var score in scores)
        {
            GameObject scoreUI = GameObject.Instantiate(ScorePrefab);
            scoreUI.transform.Find("name").GetComponent<TextMeshProUGUI>().text = score.Key;
            scoreUI.transform.Find("score").GetComponent<TextMeshProUGUI>().text = score.Value.ToString();
            scoreUI.transform.SetParent(this.transform,false);
        }
    }

}
