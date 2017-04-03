using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectedTrampoline : MonoBehaviour
{
    private float TrampolinePower = 1f;
    private float UpPower = 10;
    private float threshold = 10;
    private float SlowDownRate = 0.8f;
    public bool IsSpecial = false;
    //pass up direction of trampoline as jumpvector
    void Start()
    {
        if (IsSpecial)
        {
            UpPower = 20;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PhysicsPlayerController>().materialstate == PhysicsPlayerController.Mat.RUBBER)
            {
                var normal = this.transform.up;
                var inVelocity = other.GetComponent<PhysicsPlayerController>().GetLocalVelocity();
                Vector3 reflectedVector = inVelocity;
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
                other.GetComponent<PhysicsPlayerController>().ApplyLocalForce(reflectedVector * TrampolinePower);
            }
        }
    }
}
