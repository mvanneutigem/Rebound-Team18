using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		
	}
    //pass up direction of trampoline as jumpvector
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().ApplyForce(10);
            Debug.Log("ontrampoline");
        }
    }
}
