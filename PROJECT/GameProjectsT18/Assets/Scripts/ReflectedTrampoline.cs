using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectedTrampoline : MonoBehaviour
{
    private float TrampolinePower = 1f;
    private float UpPower = 10;
    private float threshold = 10;
    private float SlowDownRate = 0.8f;
    //pass up direction of trampoline as jumpvector
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            var normal = this.transform.up;
            var inVelocity = other.GetComponent<PlayerController>().GetVelocity();
            var reflectedVector = Vector3.Reflect(inVelocity, normal);
            reflectedVector.x = inVelocity.x;
            reflectedVector.z = inVelocity.z;
            //only reflect y part of velocity over set threshold

            reflectedVector.y = UpPower;

            //if (reflectedVector.y < threshold)
            //{
            //    reflectedVector.y = UpPower;
            //}
            //else
            //{
            //    //rate of slowdown
            //    reflectedVector.y *= SlowDownRate;
            //    if (reflectedVector.y < threshold)
            //        reflectedVector.y = threshold;
            //}

            other.GetComponent<PlayerController>().ApplyForce(reflectedVector * TrampolinePower);
        }
    }
}
