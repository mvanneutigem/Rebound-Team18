using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {

	// Use this for initialization
    private int _nextScene;
    private const float SOLID_TIME = 2.0f;
    private const float LOAD_TIME = 4.0f;
    private Text _text;
    private float _timer = 0.0f;
    private AsyncOperation _operation;
    void Start ()
	{
	    _nextScene = PlayerPrefs.GetInt("Scene") + 1;
        _text = GameObject.Find("Text").GetComponent<Text>();
        _operation = SceneManager.LoadSceneAsync(_nextScene, LoadSceneMode.Single);
        _operation.allowSceneActivation = false;
        StartLoading();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _timer += Time.deltaTime;

	    if (_timer > SOLID_TIME)
	    {
	        Color col = _text.color;
	        col.a = ((LOAD_TIME - _timer) / (LOAD_TIME - SOLID_TIME));
            _text.color = col;

	        if (_timer > LOAD_TIME)
	        {
	            ActivateScene();
	            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_nextScene));
	        }
	    }
    }
    private void StartLoading()
    {
        StartCoroutine("load");
    }

    IEnumerator load()
    {
        yield return _operation;
    }

    private void ActivateScene()
    {
        _operation.allowSceneActivation = true;
    }
}
