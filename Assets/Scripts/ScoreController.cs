using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

// Project -> Manage NuGet Packages... -> Browse -> Enter "newtonsoft.json" -> Install

public class ScoreController
{
    private static readonly string baseURI = "http://localhost:8000/api";

    public static List<Score> GetScoreById(string id)
    {
        try
        {
            string uri = String.Format(baseURI + "/score/{0}", id);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            string jsonText = streamReader.ReadToEnd();
            List<Score> scoreList = JsonConvert.DeserializeObject<List<Score>>(jsonText);
            // Test: Print to Console
            for (int i = 0; i < scoreList.Count; ++i)
            {
                Debug.Log(scoreList[i].ToString());
            }
            return scoreList;
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
    }

    public static Score GetScoreByIdAndLevel(string id, int level)
    {
        try
        {
            string uri = String.Format(baseURI + "/score/{0}/{1}", id, level);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonText = reader.ReadToEnd();
            List<Score> scoreList = JsonConvert.DeserializeObject<List<Score>>(jsonText);
            // Test: Print to Console
            Debug.Log(scoreList[0].ToString());
            return scoreList[0];
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
    }

    public static List<Score> GetScoreListByLevel(int level)
    {
        try
        {
            string uri = String.Format(baseURI + "/ranking/{0}", level);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonText = reader.ReadToEnd();
            List<Score> scoreList = JsonConvert.DeserializeObject<List<Score>>(jsonText);
            // Test: Print to Console
            for (int i = 0; i < scoreList.Count; ++i)
            {
                Debug.Log(scoreList[i].ToString());
            }
            return scoreList;
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
    }

    public static List<Score> GetSortedScoreListByLevel(int level)
    {
        try
        {
            List<Score> scoreList = GetScoreListByLevel(level);
            scoreList.Sort(
                delegate(Score score1, Score score2)
                {
                    return score1.Scorex.CompareTo(score2.Scorex);
                }
            );
            return scoreList;
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
    }

    // To-do
    public static void PostScore(string id, int score, int level)
    {
        // id: LoginToFBButton.ltfb.GetFacebookPlayer().Id
        // level: GameScreen => GetGameLevel()
        // score: Match3Game.board.userScore when GameEnd(true), but it's not real score,
        // the score still increases after the game is end.
    }
}
