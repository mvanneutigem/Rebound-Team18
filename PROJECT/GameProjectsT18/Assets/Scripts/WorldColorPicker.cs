using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldColorPicker : MonoBehaviour {

    public Material mat;
	// Use this for initialization
	void Start ()
	{
	    int Id = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(Id);
        if (Id < 10)
        {
            Color col = mat.color;
            col.r = 0;
            col.g = 0.31f;
            col.b = 0.8f;
            mat.color = col; 
        }
        else if (Id > 9 && Id < 15)
        {
            Color col = mat.color;
            col.r = 0;
            col.g = 0.85f;
            col.b = 0.1f;
            mat.color = col;
        }
        else if (Id > 14 && Id < 20)
        {
            Color col = mat.color;
            col.r = 0.9f;
            col.g = 0.45f;
            col.b = 0;
            mat.color = col;
        }
        else if (Id > 19 && Id < 25)
        {
            Color col = mat.color;
            col.r = 0;
            col.g = 0;
            col.b = 0;
            mat.color = col;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
