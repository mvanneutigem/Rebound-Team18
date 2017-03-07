using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScreen : MonoBehaviour
{
    private int _score;
    private int _count;
    private int _offset = 100;
	// Use this for initialization
	void Start ()
    {
        _score = PlayerPrefs.GetInt("Score");
        PlayerPrefs.SetInt("Score", 0);
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (_count < _score)
	        this.GetComponent<Text>().text = _count.ToString();
        else
            this.GetComponent<Text>().text = _score.ToString();

	    _count += _offset;
	}

    public void ContinueButton()
    {
        Time.timeScale = 1;
        if (PlayerPrefs.GetInt("Scene") < 7)
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Scene") + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
