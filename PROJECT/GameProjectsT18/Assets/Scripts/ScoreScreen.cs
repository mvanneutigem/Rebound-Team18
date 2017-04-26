﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
//using UnityEditor;

public class ScoreScreen : MonoBehaviour
{
    public Text NameText;
    public Text scoreText;

    private int _score;
    private int _count;
    private int _offset = 100;
    private bool _baseScore = false;
    private HighScores highScoreManager;
    public int _levelIndex;
    private bool highscore = false;
    private string lowestUser = "";
    private string name = "";
    private int NrOfNames = 0;
    private Highscore[] list;
    // Use this for initialization
    void Start ()
    {
        highscore = true;
        _levelIndex = PlayerPrefs.GetInt("Scene");
        
        _score = PlayerPrefs.GetInt("Score");
        _score += (int)(PlayerPrefs.GetFloat("time") * 100.0f);
        PlayerPrefs.SetInt("Score", 0);

        highScoreManager = GetComponent<HighScores>();
        //list = highScoreManager.GetHighscoreslist();
	    /*highScoreManager.SetLeaderboard(_levelIndex - 3);*///tutorial is at index 3
        print(_levelIndex);

	    //   int lowestScore = 10000000;

	    //int length = list.Length;
	    //   for (int i = 0; i < length; ++i)
	    //   {
	    //       if (list[i].levelIndex == _levelIndex)
	    //       {
	    //           ++NrOfNames;
	    //           if (list[i].score < lowestScore)
	    //           {
	    //               lowestScore = list[i].score;
	    //               lowestUser = list[i].username;
	    //           }
	    //           if (list[i].score < _score)
	    //           {
	    //               highscore = true;
	    //           }
	    //       }
	    //   }

	    //if (NrOfNames < 5)
	    //{
	    //       highscore = true;
	    //   }

	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_baseScore)
	    {
	        if (_count < _score)
	        {
                scoreText.GetComponent<Text>().text = _count.ToString();
	            _count += _offset;
	        }
	        else
	        {
                scoreText.GetComponent<Text>().text = _score.ToString();
	            _baseScore = true;
	        }   
        }

        //if (highscore && name != "")//check if highscore && name entered
        //{
        //    //if (NrOfNames >= 5)
        //    //    highScoreManager.DeleteHighscore(lowestUser);
        //    highScoreManager.AddNewHighscore(name , _score, _levelIndex);
        //    highscore = false;
        //}
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        if (_levelIndex < 7)//max levelindex for now
        {
            SceneManager.LoadScene(_levelIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    public void LoadScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public void NameEntered()
    {
        if (highscore)
        {
            name = NameText.text;
            highScoreManager.AddNewHighscore(name, _score);
            highscore = false;
        }
    }
}
