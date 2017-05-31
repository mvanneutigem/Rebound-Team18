using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterRotate : MonoBehaviour {
    private Transform _transSelf;
    public float Speed;
	// Use this for initialization
	void Start () {
        _transSelf = this.transform;
		
	}
	
	// Update is called once per frame
	void Update () {
        _transSelf.Rotate(0f, Speed, 0f);
    }
}
