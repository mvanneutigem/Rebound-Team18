using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;
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
    public GameObject switchScript;

    public int myScore = 100000;

    private List<object> Scorelist = null;
    private string AppLinkURL = "https://apps.facebook.com/rebound_";

    private bool _Init = false;
    private bool _gotScore = false;
    private bool _AllPermissions = false;
    private string _myID = "";

    void Awake()
    {
        Init();
    }

    void Start()
    {

        DealWithFBMenus(FB.IsLoggedIn);
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

    public void FBlogout()
    {
        FB.LogOut();
        DealWithFBMenus(FB.IsLoggedIn);
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
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            if (LoggedInCanvas)
            {
                LoggedInCanvas.gameObject.SetActive(true);
                LoggedOutCanvas.gameObject.SetActive(false);
                FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
                FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
                DisplayScore();
            }
            else
            {
                Debug.Log("Called log in on non canvas");
                //FB.API("/me?fields=score", HttpMethod.GET, GetCurrentScore);
            }
        }
    }

    public void WorldSwitcher()
    {
        Debug.Log("Called logged in on switcher");
        if(FB.IsLoggedIn)
            FB.API("/me/scores", HttpMethod.GET, GetCurrentScore);
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
        if (LoggedInCanvas)
        {
            if (isLoggedIn)
            {
                Debug.Log("Logged in");
                if (SceneManager.GetActiveScene().buildIndex == 2) //scorescreen needs more permissions
                {
                    Debug.Log("checking permissions");
                    FB.API("/me/permissions",
                        HttpMethod.GET,
                        CheckPermissions
                        );
                }
                else
                {
                    //fb is logged in
                    LoggedInCanvas.gameObject.SetActive(true);
                    LoggedOutCanvas.gameObject.SetActive(false);
                    FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
                    FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
                }


            }
            else
            {
                Debug.Log("Not logged in");
                //fb not logged in
                LoggedInCanvas.gameObject.SetActive(false);
                LoggedOutCanvas.gameObject.SetActive(true);
            }
        }
       
    }

    private void CheckPermissions(IGraphResult result)
    {
        IDictionary<string, object> data = result.ResultDictionary;
        Debug.Log(result.RawResult);
        List<object> permissionsList = (List<object>) data["data"];
        bool grantedFriends = false;
        bool grantedPublish = false;

        foreach (object obj in permissionsList)
        {
            var entry = (Dictionary<string, object>) obj;
            var permission = entry["permission"];
            var status = entry["status"];
            if (permission.ToString() == "user_friends" && status.ToString() == "granted")
            {
                grantedFriends = true;
            }
            if (permission.ToString() == "publish_actions" && status.ToString() == "granted")
            {
                grantedPublish = true;
            }
        }
        if (grantedPublish && grantedFriends)
        {
            //fb is logged in
            LoggedInCanvas.gameObject.SetActive(true);
            LoggedOutCanvas.gameObject.SetActive(false);
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
            DisplayScore();

        }
        else
        {
            Debug.Log("permissions declined");
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
            "Check out this game!",
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
            "Bet you can't beat my score!",
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
    //----------------
    public int getScore()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Logged in!");
            QueryScores();
            return myScore;
        }
        else
        {
            Debug.Log("Not logged in");
            return -1;
        }

    }
    public void QueryScores()
    {
        FB.API("/app/scores?fields=score,user.limit(30)",HttpMethod.GET, ScoresCallback);
        FB.API("/me/scores?fields=score", HttpMethod.GET, GetCurrentScore);
    }
    void GetCurrentScore(IResult result)
    {
        Debug.Log("called get current score method");
        IDictionary<string, object> data = result.ResultDictionary;
        Scorelist = (List<object>)data["data"];

        var entry = (Dictionary<string, object>)Scorelist[0];
        var user = (Dictionary<string, object>)entry["user"];
        _myID = user["id"].ToString();
        string scorestring = entry["score"].ToString();
        int sc = int.Parse(scorestring);
        PlayerPrefs.SetInt("Highscore", sc);
        if (switchScript)
            switchScript.GetComponent<SwitchWorlds>().SetButtons(0);

    }
    private void ScoresCallback(IResult result)
    {
        IDictionary<string, object> data = result.ResultDictionary;
        Scorelist = (List<object>) data["data"];

        foreach (Transform child in ScoreScrollList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (object obj in Scorelist)
        {
            var entry = (Dictionary<string, object>) obj;
            var user = (Dictionary<string, object>) entry["user"];

            GameObject scorePanel;
            scorePanel = Instantiate(ScoreEntryPanel) as GameObject;
            scorePanel.transform.parent = ScoreScrollList.transform;

            Transform ThisScoreName = scorePanel.transform.Find("name");
            Transform ThisScoreScore = scorePanel.transform.Find("score");

            Text scoreName = ThisScoreName.GetComponent<Text>();
            Text scoreScore = ThisScoreScore.GetComponent<Text>();

            scoreName.text = user["name"].ToString();
            scoreScore.text = entry["score"].ToString();
            if (_myID == user["id"].ToString())
            {
                myScore = int.Parse(scoreScore.text);
            }

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

        Debug.Log("Got user ID");
        _gotScore = true;
    }

    public void SetScore(int score)
    {
        Highscores.GetComponent<ScoreScreen>().UnlockLevel(myScore);
        if (myScore < score)
        {
            var scoreData = new Dictionary<string, string>();
            scoreData["score"] = score.ToString();

            FB.API("/me/scores", HttpMethod.POST, delegate (IGraphResult result)
            {
                Debug.Log("Score submit result: " + result.RawResult);
                QueryScores();

            }, scoreData);
        }
    }

    public void DisplayScore()
    {
        QueryScores();
    }


    //Achievements
    //------------



}
