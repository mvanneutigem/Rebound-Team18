using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour {

    private float speedMultiplier = 60.0f;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var speedVector = other.GetComponent<PlayerController>().GetVelocity();
            var speed = Vector3.forward * speedMultiplier;
            speedVector += speed;
            other.GetComponent<PlayerController>().SetVelocity(speedVector);
        }
    }
}
