using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Controls : MonoBehaviour
{

    public Dropdown DropdownInputRewind;
    public Dropdown DropdownInputSlam;
    // Update is called once per frame
    void Start()
    {
        Debug.Log("start");
        var i = PlayerPrefs.GetString("Slam");
        switch (i)
        {
            case "space":
                DropdownInputSlam.value = 0;
                break;
            case "left shift":
                DropdownInputSlam.value = 1;
                break;
            case "e":
                DropdownInputSlam.value = 2;
                break;
        }

        var j = PlayerPrefs.GetString("Rewind");
        switch (j)
        {
            case "r":
                DropdownInputRewind.value = 0;
                break;
            case "left shift":
                DropdownInputRewind.value = 1;
                break;
            case "e":
                DropdownInputRewind.value = 2;
                break;
        }
    }

	public void SlamChange ()
	{
	    int i = DropdownInputSlam.value;
	    switch (i)
	    {
            case 0: PlayerPrefs.SetString("Slam", "space" );
                Debug.Log("space");
	            break;
            case 1:
                PlayerPrefs.SetString("Slam", "left shift");
                Debug.Log("shift");
                break;
            case 2:
                PlayerPrefs.SetString("Slam", "e");
                Debug.Log("e");
                break;
        }
        
	}

    public void rewindChange()
    {
        int i = DropdownInputRewind.value;
        switch (i)
        {
            case 0:
                PlayerPrefs.SetString("Rewind", "r");
                break;
            case 1:
                PlayerPrefs.SetString("Rewind", "left shift");
                break;
            case 2:
                PlayerPrefs.SetString("Rewind", "e");
                break;
        }

    }
}
