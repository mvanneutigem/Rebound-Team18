using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{

    public Text[] textArray;
    public float[] positionArray;
    public float[] distanceArray;
    private GameObject _player;

    // Use this for initialization
    void Start ()
    {
        _player = GameObject.FindWithTag("Player");
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            textArray[0].text = "Press " + PlayerPrefs.GetString("Slam") + "/A button to launch";
            textArray[3].text = "Use " + PlayerPrefs.GetString("Slam") + "/A button to slam";
            textArray[4].text = "Use " + PlayerPrefs.GetString("Rewind") + "/X button to rewind";
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
	    for (var i = 0; i < textArray.Length; ++i)
	    {
            textArray[i].gameObject.SetActive(false);
            if (_player.transform.position.z > positionArray[i] && _player.transform.position.z  < positionArray[i] + distanceArray[i] )
	        {
	            textArray[i].gameObject.SetActive(true);

	        }
	    }
	}
}
