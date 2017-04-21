using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
