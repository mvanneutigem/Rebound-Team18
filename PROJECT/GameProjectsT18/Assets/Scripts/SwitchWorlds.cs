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

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;

    void Start()
    {
        SetButtons(0);
        FB.GetComponent<FBScript>().WorldSwitcher();
        SetButtons(0);
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
        GameObject myEventSystem = GameObject.Find("EventSystem");
        switch (currentWorld)
        {
            case 1:
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(button1);
                panelArr[0].SetActive(true);
                panelArr[1].SetActive(false);
                panelArr[2].SetActive(false);
                panelArr[3].SetActive(false);
                SetButtons(0);
                break;
            case 2:
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(button2);
                panelArr[0].SetActive(false);
                panelArr[1].SetActive(true);
                panelArr[2].SetActive(false);
                panelArr[3].SetActive(false);
                SetButtons(1);
                break;
            case 3:
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(button3);
                panelArr[0].SetActive(false);
                panelArr[1].SetActive(false);
                panelArr[2].SetActive(true);
                panelArr[3].SetActive(false);
                SetButtons(2);
                break;
            case 4:
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(button4);
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
