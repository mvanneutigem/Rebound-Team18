using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    //FIELDS
    public Transform panel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        //PlayerPrefs.SetString("Slam", "space");
        //PlayerPrefs.SetString("Rewind", "r");
        //PlayerPrefs.SetInt("Highscore", 0);
    }
    //void Start()
    //{
    //    if (PlayerPrefs.GetInt("SetDefault") != 1 || PlayerPrefs.GetInt("Launched") != 1)
    //    {
    //        PlayerPrefs.SetString("Slam", "space");
    //        PlayerPrefs.SetString("Rewind", "r");
    //        PlayerPrefs.SetInt("SetDefault", 1);
    //    }

    //    if (PlayerPrefs.GetInt("Launched") != 1)
    //    {
    //        PlayerPrefs.SetInt("Launched", 1);
    //    }

    //}

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartSession(int sceneIndex)
    {
        PlayerPrefs.SetInt("Scene", 4);
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex);
    }

    public void loadScene(int sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(PlayerPrefs.GetInt("Scene"));
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
