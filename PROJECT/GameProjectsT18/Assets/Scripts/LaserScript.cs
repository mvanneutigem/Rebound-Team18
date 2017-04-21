using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    // Use this for initialization
    public Transform StartPoint;
    public Transform EndPoint;
    LineRenderer laserLine;
    public float LaserWidth = 0.2f;
    void Start () {
        laserLine = GetComponent<LineRenderer>();
        laserLine.SetWidth(LaserWidth, LaserWidth);
	}
	
	// Update is called once per frame
	void Update () {
        laserLine.SetPosition(0, StartPoint.position);
        laserLine.SetPosition(1, EndPoint.position);

    }
}
