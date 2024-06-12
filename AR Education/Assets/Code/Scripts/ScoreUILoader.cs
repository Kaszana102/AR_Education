using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUILoader : MonoBehaviour
{

    [SerializeField]
    Game gameToLoadScores = Game.LATTICE;

    [SerializeField]
    Transform ScoreContainer;

    [SerializeField]
    GameObject ScorePrefab;

    void Awake()
    {
        var scores = ScoreManager.GetGameScores(gameToLoadScores);

        foreach(var score in scores)
        {
            GameObject scoreUI = GameObject.Instantiate(ScorePrefab);
            scoreUI.transform.Find("name").GetComponent<TextMeshProUGUI>().text = score.Key;
            scoreUI.transform.Find("score").GetComponent<TextMeshProUGUI>().text = score.Value.ToString();
            scoreUI.transform.SetParent(ScoreContainer, false);
        }

        Vector2 delta = ScoreContainer.GetComponent<RectTransform>().sizeDelta;
        delta.y = ScorePrefab.GetComponent<RectTransform>().sizeDelta.y * scores.Count;
        ScoreContainer.GetComponent<RectTransform>().sizeDelta = delta;
    }

}
