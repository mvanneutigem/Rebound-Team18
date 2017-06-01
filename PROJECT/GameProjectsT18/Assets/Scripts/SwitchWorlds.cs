using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWorlds : MonoBehaviour
{

    public GameObject[] panelArr;
    public GameObject FB;
    private int currentWorld = 1;
    private int maxworld = 4;

    void Start()
    {
        SetButtons(0);
        FB.GetComponent<FBScript>().WorldSwitcher();
    }

    public void UnlockAll()
    {
        PlayerPrefs.SetInt("Highscore", 1000000);
        SetButtons(0);
    }

    public void SetButtons(int id)
    {
        int baseScore = 10000;
        for (int i = 0; i < 5; ++i)
        {
            if (PlayerPrefs.GetInt("Highscore") < baseScore * (i + (currentWorld-1) * 5))
            {
                panelArr[id].transform.GetChild(i+2).GetComponent<Button>().interactable = false;
            }
            else
            {
                panelArr[id].transform.GetChild(i+2).GetComponent<Button>().interactable = true;
            }
        }
    }
    private void SwitchWorld()
    {
        switch (currentWorld)
        {
            case 1:
                panelArr[0].SetActive(true);
                panelArr[1].SetActive(false);
                panelArr[2].SetActive(false);
                panelArr[3].SetActive(false);
                SetButtons(0);
                break;
            case 2:
                panelArr[0].SetActive(false);
                panelArr[1].SetActive(true);
                panelArr[2].SetActive(false);
                panelArr[3].SetActive(false);
                SetButtons(1);
                break;
            case 3:
                panelArr[0].SetActive(false);
                panelArr[1].SetActive(false);
                panelArr[2].SetActive(true);
                panelArr[3].SetActive(false);
                SetButtons(2);
                break;
            case 4:
                panelArr[0].SetActive(false);
                panelArr[1].SetActive(false);
                panelArr[2].SetActive(false);
                panelArr[3].SetActive(true);
                SetButtons(3);
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
