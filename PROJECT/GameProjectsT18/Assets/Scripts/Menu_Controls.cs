using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Controls : MonoBehaviour
{

    public Dropdown DropdownInput;
	// Update is called once per frame

	public void SlamChange ()
	{
	    int i = DropdownInput.value;
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
        int i = DropdownInput.value;
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
