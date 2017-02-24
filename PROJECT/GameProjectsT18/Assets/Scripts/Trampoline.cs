﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private float TrampolinePower = 10;
    //pass up direction of trampoline as jumpvector
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var up = this.transform.up;
            other.GetComponent<PlayerController>().ApplyForce(TrampolinePower * up);
            Debug.Log("ontrampoline");
        }
    }
}