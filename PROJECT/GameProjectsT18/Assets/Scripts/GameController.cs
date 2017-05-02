using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour {

    private Text _scoreText;
    private Text _timeText;
    private int _score;
    private float _timer;
    private bool _gameStarted = false;

    public void StartGame()
    {
        _gameStarted = true;
    }
    // Use this for initialization
    void Start ()
    {
        _scoreText = GameObject.FindWithTag("ScoreText").GetComponent<Text>();
        _timeText = GameObject.FindWithTag("TimeText").GetComponent<Text>();
        _score = 0;
        _timer = 120;
    }

    void Update()
    {
        if (_gameStarted)
        {
            _timer -= Time.deltaTime;
        }
        _timeText.text = _timer.ToString("n2");

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

    public float GetTime()
    {
        return _timer;
    }
}
