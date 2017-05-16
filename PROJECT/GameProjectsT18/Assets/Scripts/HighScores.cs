using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    private string privateCode = "";
    private string publicCode = "";
    private string webURL = "http://dreamlo.com/lb/";//making use of free database service for leaderboards

    public Highscore[] Highscoreslist;
    private DisplayHighScores highScoresDisplay;

    public void SetLeaderboard(int levelIndex)
    {
        switch (levelIndex)
        {
            case 0:
                privateCode = "OvNQ014guUO730IidxyQHQIICY6VxVFkeX3iSNBHWDMQ";
                publicCode = "58c46396d60245055cd4370d";
                break;
            case 1:
                privateCode = "3WM5ptDqqUiIDwDWEFAAxgM2cjeA1E5kGT_zc9Ed-EEQ";
                publicCode = "58c48161d60245055cd4565a";
                break;
            case 2:
                privateCode = "nh63SQ3l-Emt82bDLlRDaAC8PX-sj940WK6dVwo4TEEA";
                publicCode = "58c481dbd60245055cd456dc";
                break;
            case 3:
                privateCode = "SsK-uYPebkahQ2ZlTdzm9AxDFfC68_X06oXc2qy-wFpA";
                publicCode = "58c48214d60245055cd45708";
                break;
            case 4:
                privateCode = "TJlF5uqeFk-2MY-ktqCApA2qPbWbdoNEi-6551Q30Jhw";
                publicCode = "58c48228d60245055cd4571a";
                break;
        }
    }
    void Awake()
    {
        highScoresDisplay = GetComponent<DisplayHighScores>();
    }
    public void AddNewHighscore(string username, int score)
    {
        StartCoroutine(UploadNewHighscore(username, score));
    }
    IEnumerator UploadNewHighscore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode +  "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            print("error uploading " + www.error);
        }
        else
        {
            DownloadHighscores();//keep scores up to date
        }
    }

    public void DownloadHighscores()
    {
        StartCoroutine(DownloadHighScoresFromDatabase());
    }

    IEnumerator DownloadHighScoresFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscore(www.text);
            highScoresDisplay.OnHighscoresDownloaded(Highscoreslist);
        }
        else
        {
            print("error downloading " + www.error);
        }
    }

    void FormatHighscore(string textstream)
    {
        string[] entries = textstream.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
        Highscoreslist = new Highscore[entries.Length];
        for (int i = 0; i < entries.Length; ++i)
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
           
            if (entryInfo.Length > 1)
            {
                string username = entryInfo[0];
                username = username.Replace("+", " ");
                int score = int.Parse(entryInfo[1]);
                Highscoreslist[i] = new Highscore(username, score);
            }
            
        }
    }

    public void DeleteHighscore(string username)
    {
        StartCoroutine(DeleteHighScore(username));
    }

    IEnumerator DeleteHighScore(string username)
    {
        WWW www = new WWW(webURL + privateCode + "/delete/" + WWW.EscapeURL(username));
        yield return www;
    }

    public Highscore[] GetHighscoreslist()
    {
        DownloadHighscores();
        return Highscoreslist;
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}
