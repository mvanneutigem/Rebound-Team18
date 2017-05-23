using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWorlds : MonoBehaviour
{

    public GameObject panel1;
    public GameObject panel2;
    private int currentWorld = 1;
    private int maxworld = 2;

    void Start()
    {
        SetButtons(panel1);
    }

    public void UnlockAll()
    {
        PlayerPrefs.SetInt("Highscore", 1000000);
        SetButtons(panel1);
    }

    void SetButtons(GameObject panel)
    {
        int baseScore = 10000;
        for (int i = 0; i < 5; ++i)
        {
            if (PlayerPrefs.GetInt("Highscore") < baseScore * (i + (currentWorld-1) * 5))
            {
                panel.transform.GetChild(i+2).GetComponent<Button>().interactable = false;
            }
            else
            {
                panel.transform.GetChild(i+2).GetComponent<Button>().interactable = true;
            }
        }
    }
    private void SwitchWorld()
    {
        switch (currentWorld)
        {
            case 1:
                panel1.SetActive(true);
                panel2.SetActive(false);
                SetButtons(panel1);
                break;
            case 2:
                panel1.SetActive(false);
                panel2.SetActive(true);
                SetButtons(panel2);
                break;
        }
    }

    public void NextWorld()
    {
        if (currentWorld != maxworld)
        {
            currentWorld += 1;
        }
        SwitchWorld();
    }

    public void previousWorld()
    {
        if (currentWorld > 1)
        {
            currentWorld -= 1;
        }
        SwitchWorld();
    }
}
