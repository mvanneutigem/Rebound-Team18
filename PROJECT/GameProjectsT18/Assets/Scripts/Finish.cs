using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private GameController _gameController;
    public int ScoreSceneIndex;

    void Start()
    {
        GameObject gameControllerObj = GameObject.FindWithTag("GameController");
        if (gameControllerObj != null)
        {
            _gameController = gameControllerObj.GetComponent<GameController>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerPrefs.SetFloat("time", _gameController.GetTime());
            PlayerPrefs.SetInt("Scene", SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(ScoreSceneIndex);
        }
    }
}
