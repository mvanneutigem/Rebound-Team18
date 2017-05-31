using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour {

	// Use this for initialization
    private int previousId = 0;
    private int menuId = 0;
	void Start () {
		DontDestroyOnLoad(this);
        previousId = SceneManager.GetActiveScene().buildIndex;
	    menuId = previousId;
	}
	
	// Update is called once per frame
	void Update () {
        int Id = SceneManager.GetActiveScene().buildIndex;

	    if (Id != previousId)
	    {
            this.gameObject.name = "MusicPersistent";

	        if (Id > 4)
	        {
	            DestroyObject(this.gameObject);
	        }
	        else if (Id == menuId)
	        {
	            Destroy(GameObject.Find("Music"));
	        }
	    }



        previousId = Id;
	}
}
