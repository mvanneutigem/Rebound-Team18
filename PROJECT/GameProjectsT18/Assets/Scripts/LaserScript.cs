using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    // Use this for initialization
    public Transform StartPoint;
    public Transform EndPoint;
    LineRenderer laserLine;
    void Start () {
        laserLine = GetComponent<LineRenderer>();
        laserLine.SetWidth(.2f, .2f);
	}
	
	// Update is called once per frame
	void Update () {
        laserLine.SetPosition(0, StartPoint.position);
        laserLine.SetPosition(1, EndPoint.position);

    }
}
