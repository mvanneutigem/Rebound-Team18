using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    public int ScoreSceneIndex;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerPrefs.SetInt("Scene", SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(ScoreSceneIndex);
        }
    }
}
