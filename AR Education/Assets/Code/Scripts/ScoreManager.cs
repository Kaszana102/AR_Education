using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public enum Game
{
    LATTICE,
    BEZIER    
}

public class Score
{
    public string challengeName="";
    public int score=0;
}

/// <summary>
/// Class for managing games scores. it is supposed to work with any game.
/// There is no removing scores, because there is no need to.
/// </summary>
static public class ScoreManager
{
    const string LATTICE_FILE = "lattice_scores";
    const string BEZIER_FILE = "bezier_scores";

    static string GetGameScoreFileName(Game game)
    {
        switch (game)
        {
            case Game.LATTICE:
                return LATTICE_FILE;
            case Game.BEZIER:
                return BEZIER_FILE;
            default: 
                return "ERROR";
        }
    }

    static void CheckFileExistence(Game game)
    {
        switch (game)
        {
            case Game.LATTICE:
                if (!File.Exists(LATTICE_FILE))
                {
                    File.Create(LATTICE_FILE);
                }
                    break;
            case Game.BEZIER:
                if (!File.Exists(BEZIER_FILE))
                {
                    File.Create(BEZIER_FILE);
                }
                break;
        }
        
    }    

    public static Dictionary<string,int> GetGameScores(Game game)
    {
        CheckFileExistence(game);
        using (StreamReader r = new StreamReader(GetGameScoreFileName(game)))
        {
            string json = r.ReadToEnd();
            Dictionary<string, int> items = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
            return items;
        }                
    }

    /// <summary>
    /// If score already exists (for certain challenge) it replaces it with better score
    /// </summary>
    /// <param name="score"></param>
    /// <param name="game"></param>
    public static void AddScore(Score score,Game game)
    {
        var scores = GetGameScores(game);

        if (scores.ContainsKey(score.challengeName) && scores[score.challengeName] < score.score)
        {
            scores[score.challengeName] = score.score;
        }
        else
        {
            scores.Add(score.challengeName, score.score);
        }
        SaveScores(game, scores);
    }
    
    static void SaveScores(Game game, Dictionary<string, int> scores)
    {
        using (StreamWriter r = new StreamWriter(GetGameScoreFileName(game)))
        {
            string json = JsonConvert.SerializeObject(scores);
            r.Write(json);
        }
    }

}
