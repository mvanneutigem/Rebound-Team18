using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectedTrampoline : MonoBehaviour
{
    private AudioManager _audioManager;
    private float TrampolinePower = 1f;
    public float UpPower = 10;
    private float threshold = 10;
    private float SlowDownRate = 0.8f;
    //pass up direction of trampoline as jumpvector
    void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
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

                other.GetComponent<PhysicsPlayerController>().ApplyLocalForce(reflectedVector * TrampolinePower);
                _audioManager.PlaySFX("bounce");
            }
        }
    }
}
