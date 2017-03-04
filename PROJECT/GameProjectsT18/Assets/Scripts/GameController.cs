using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    private Text _scoreText;
    private int _score;

    // Use this for initialization
    void Start ()
    {
        _scoreText = GameObject.FindWithTag("ScoreText").GetComponent<Text>();
        _score = 0;
    }
	
    void UpdateScore()
    {
        _scoreText.text = _score.ToString();
        PlayerPrefs.SetInt("Score", _score);
    }

    public void AddScore(int newScore)
    {
        _score += newScore;
        UpdateScore();
    }
}
