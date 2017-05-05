﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBScript : MonoBehaviour
{
    public GameObject LoggedOutCanvas;
    public GameObject LoggedInCanvas;
    public GameObject DialogUsername;
    public GameObject DialogProfilePic;
    public GameObject ScoreEntryPanel;
    public GameObject ScoreScrollList;
    public GameObject Highscores;

    public int myScore = 0;

    public Text ScoresDebug;

    private List<object> Scorelist = null;
    private string AppLinkURL = "https://apps.facebook.com/rebound_";

    private bool _Init = false;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (!_Init)
        {
            FB.Init(SetInit, OnHideUnity);
            _Init = true;
        }
            
    }

    private void SetInit()
    {
        DealWithFBMenus(FB.IsLoggedIn);
    }

    private void OnHideUnity(bool isGameShown)
    {
        //pause game when game isn't shown
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FBlogin()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    public void FBloginWithPermissions()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        permissions.Add("user_friends");
        permissions.Add("publish_actions");
        FB.LogInWithReadPermissions(permissions, AuthCallBackScores);
    }
    void AuthCallBackScores(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            DealWithFBMenus(FB.IsLoggedIn);
            DisplayScore();
        }
    }

    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            DealWithFBMenus(FB.IsLoggedIn);
        }
    }

    void DealWithFBMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            //fb is logged in
            LoggedInCanvas.gameObject.SetActive(true);
            LoggedOutCanvas.gameObject.SetActive(false);
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
        else
        {
            //fb not logged out
            LoggedInCanvas.gameObject.SetActive(false);
            LoggedOutCanvas.gameObject.SetActive(true);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Image ProfilePic = DialogProfilePic.GetComponent<Image>();

            ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0,0,128,128), new Vector2() );
            Debug.Log("setProfilepic");
        }

    }

    void DisplayUsername(IResult result)
    {
        Text userName = DialogUsername.GetComponent<Text>();
        if (result.Error == null)
        {
            userName.text = "Hi there,  " + result.ResultDictionary["first_name"];
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    public void Share()
    {
        FB.FeedShare(
            string.Empty,
            new Uri(AppLinkURL),
            string.Empty,
            string.Empty,
            string.Empty,
            new Uri("http://mariekevanneutigem.nl/projects/images/rebound_image.png"),
            string.Empty,
            ShareCallBack
            );
    }

    void ShareCallBack(IResult result)
    {
        if (result.Cancelled)
        {
            Debug.Log("share cancelled");
        }else if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("error on share");
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            Debug.Log("succes on share!");
        }
    }

    public void Invite()
    {
        FB.Mobile.AppInvite(
            new Uri(AppLinkURL),
            new Uri("http://mariekevanneutigem.nl/projects/images/rebound_image.png"),
            InviteCallBack
            );
    }

    void InviteCallBack(IResult result)
    {
        if (result.Cancelled)
        {
            Debug.Log("invite cancelled");
        }
        else if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("error on invite");
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            Debug.Log("succes on invite!");
        }
    }

    public void ShareWithUsers()
    {
        FB.AppRequest(
            null,
            null,
            new List<object>() { "app_users"},
            null,
            null,
            null,
            null,
            SharewWithUsersCallback
            );
    }

    void SharewWithUsersCallback(IAppRequestResult result)
    {
        Debug.Log(result.RawResult);
    }

    //scores API stuff

    public void QueryScores()
    {
        FB.API("/app/scores?fields=score,user.limit(30)",HttpMethod.GET, ScoresCallback);
        FB.API("/me/scores?fields=score", HttpMethod.GET, GetCurrentScore);
    }
    void GetCurrentScore(IResult result)
    {
        IDictionary<string, object> data = result.ResultDictionary;
        var list = (List<object>)data["data"];

        Debug.Log(list);

        string score = "";
        foreach (object obj in list)
        {
            var entry = (Dictionary<string, object>)obj;
            score = entry["score"].ToString();
            Debug.Log(score);
        }
            
        myScore = int.Parse(score);
    }
    private void ScoresCallback(IResult result)
    {
        IDictionary<string, object> data = result.ResultDictionary;
        Scorelist = (List<object>) data["data"];

        ScoresDebug.text = "";

        foreach (Transform child in ScoreScrollList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (object obj in Scorelist)
        {
            var entry = (Dictionary<string, object>) obj;
            var user = (Dictionary<string, object>) entry["user"];

            ScoresDebug.text = ScoresDebug.text + "UN: " + user["name"] + " - " + entry["score"] + "\n";

            GameObject scorePanel;
            scorePanel = Instantiate(ScoreEntryPanel) as GameObject;
            scorePanel.transform.parent = ScoreScrollList.transform;

            Transform ThisScoreName = scorePanel.transform.Find("name");
            Transform ThisScoreScore = scorePanel.transform.Find("score");

            Text scoreName = ThisScoreName.GetComponent<Text>();
            Text scoreScore = ThisScoreScore.GetComponent<Text>();

            scoreName.text = user["name"].ToString();
            scoreScore.text = entry["score"].ToString();

            //string userID = "";
            //FB.API("/me?fields=id", HttpMethod.GET, delegate (IGraphResult pictureResult)
            //{
            //    userID = result.ResultDictionary["id"].ToString();
            //});

            //if (user["id"].ToString() == userID)
            //{
            //    myScore = int.Parse(entry["score"].ToString());
            //}



            Transform ThisScoreAvatar = scorePanel.transform.Find("avatar");
            Image ScoreAvatar = ThisScoreAvatar.GetComponent<Image>();

            string id = user["id"].ToString();
            FB.API("/" + id + "/picture?type=square&height=128&width=128", HttpMethod.GET,
                delegate(IGraphResult pictureResult)
                {
                    //add error check maybe
                    ScoreAvatar.sprite = Sprite.Create(pictureResult.Texture, new Rect(0,0,128,128), new Vector2()); 
                });
        }
        Highscores.GetComponent<ScoreScreen>().AddScore();

    }

    public void SetScore(int score)
    {
        if (myScore < score)
        {
            var scoreData = new Dictionary<string, string>();
            scoreData["score"] = score.ToString();

            FB.API("/me/scores", HttpMethod.POST, delegate (IGraphResult result)
            {
                Debug.Log("Score submit result: " + result.RawResult);
            }, scoreData);
        }
    }

    public void DisplayScore()
    {
        StartCoroutine(Query());
        //scores loading
        //QueryScores();

        //if (Highscores.GetComponent<ScoreScreen>().scoreSet)
        //{
        //    QueryScores();
        //}

    }

    IEnumerator Query()
    {
        QueryScores();
        yield return new WaitForSeconds(3);//give time to fetch data
        QueryScores();
    }
}