using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class ScoreScreen : MonoBehaviour
{
    private int _score;
    private int _count;
    private int _offset = 100;
    private bool _baseScore = false;
    private bool _highscore = false;
	// Use this for initialization
	void Start ()
    {
        _score = PlayerPrefs.GetInt("Score");
        _score += (int)PlayerPrefs.GetFloat("time") * 100;
        PlayerPrefs.SetInt("Score", 0);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_baseScore)
	    {
	        if (_count < _score)
	        {
	            this.GetComponent<Text>().text = _count.ToString();
	            _count += _offset;
	        }
	        else
	        {
                this.GetComponent<Text>().text = _score.ToString();
	            _baseScore = true;
	        }   
        }
	    
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

    //working on it:
    static void WriteString()
    {
        string path = "Assets/resources/highscores.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test");
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        //TextAsset asset = Resources.Load("test");

        //Print the text from the file
        //Debug.Log(asset.text);
    }

    static void ReadString()
    {
        string path = "Assets/resources/highscores.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}
