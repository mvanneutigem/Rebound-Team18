using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWorlds : MonoBehaviour
{

    public GameObject panel1;
    public GameObject panel2;
    private int currentWorld = 1;
    private int maxworld = 2;

    private void SwitchWorld()
    {
        switch (currentWorld)
        {
            case 1:
                panel1.SetActive(true);
                panel2.SetActive(false);
                break;
            case 2:
                panel1.SetActive(false);
                panel2.SetActive(true);
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
