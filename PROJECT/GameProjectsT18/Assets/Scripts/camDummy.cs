using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camDummy : MonoBehaviour {

    // Use this for initialization
    private PlayerController _player;
	void Start () {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = _player.transform.position;
        transform.up = _player.GetUpVector();
    }
}
