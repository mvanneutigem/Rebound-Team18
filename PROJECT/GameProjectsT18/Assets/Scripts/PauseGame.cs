using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour
{
    //FIELDs
    public Transform panel;

    //METHODS
    void Update()
    {
        if (Input.GetButtonUp("Pause"))
        {
            if (panel.gameObject.activeInHierarchy == false)
            {
                panel.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                panel.gameObject.SetActive(false);
            }
        }
    }
}
