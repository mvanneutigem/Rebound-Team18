using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    //FIELDS
    public Transform panel;
    void Start()
    {
        if (PlayerPrefs.GetInt("SetDefault") != 1)
        {
            PlayerPrefs.SetString("Slam", "space");
            PlayerPrefs.SetString("Rewind", "r");
            PlayerPrefs.SetInt("SetDefault", 1);
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void loadScene(int sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        panel.gameObject.SetActive(false);
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
