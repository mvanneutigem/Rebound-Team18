using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public void LoadScene(int scene)
    {
        PlayerPrefs.SetInt("Scene", scene - 1);
        SceneManager.LoadScene(4);
    }
}
