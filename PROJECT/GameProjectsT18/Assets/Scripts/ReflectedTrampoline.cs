using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectedTrampoline : MonoBehaviour
{
    private float TrampolinePower = 1f;
    private float UpPower = 10;
    //pass up direction of trampoline as jumpvector
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            var normal = this.transform.up;
            var inVelocity = other.GetComponent<PlayerController>().GetWorldSpaceVelocity();
            var reflectedVector = Vector3.Reflect(inVelocity, normal);
            if (reflectedVector.y < 10.0f)
            {
                reflectedVector.y = UpPower;
            }
            else
            {
                reflectedVector.y *= 0.8f;
            }
            Debug.Log("add Up, " + reflectedVector);
            Debug.Log("in, " + inVelocity);
            other.GetComponent<PlayerController>().ApplyForce(reflectedVector * TrampolinePower);
            Debug.Log("ontrampoline");
        }
    }
}
