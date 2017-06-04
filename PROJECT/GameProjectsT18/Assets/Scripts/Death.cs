using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private bool _enabled = true;
    public void SetEnabled(bool enabled)
    {
        _enabled = enabled;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && _enabled)
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        }
    }
}
