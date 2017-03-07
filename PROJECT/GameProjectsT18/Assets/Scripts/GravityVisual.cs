using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityVisual : MonoBehaviour {
    private PlayerController _playerController;
    private Vector3 _originalPos;
    private Transform _transSelf;
    // Use this for initialization
    void Start () {
        GameObject gameControllerObj = GameObject.FindWithTag("Player");
        _transSelf = this.transform;
        _originalPos = _transSelf.position;
    }
	
	// Update is called once per frame
	void Update () {
        _transSelf.Rotate(0f, 0f, 1.0f);
    }
}
