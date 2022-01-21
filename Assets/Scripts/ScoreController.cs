using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

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
        catch (Exception ex)
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
        catch (Exception ex)
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
                delegate (Score score1, Score score2)
                {
                    return score1.score.CompareTo(score2.score);
                }
            );
            return scoreList;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
    }

    // To-do
    public static async System.Threading.Tasks.Task PostScoreAsync(string id, int score, int level)
    {
        try
        {
            Score test = new Score(id, score, level);
            string uri = String.Format(baseURI + "/score/");
            var jsonData = JsonConvert.SerializeObject(test);
            var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.PostAsync(uri, httpContent);
            if (httpResponse != null)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                Debug.Log(responseContent);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}