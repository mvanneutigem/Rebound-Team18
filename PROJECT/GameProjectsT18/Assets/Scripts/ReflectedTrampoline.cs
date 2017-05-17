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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PhysicsPlayerController>().materialstate == PhysicsPlayerController.Mat.RUBBER)
            {
                var normal = this.transform.up;
                var inVelocity = other.gameObject.GetComponent<PhysicsPlayerController>().GetLocalVelocity();
                Vector3 reflectedVector = inVelocity;
                //only reflect y part of velocity over set threshold

                reflectedVector.y = UpPower;

                other.gameObject.GetComponent<PhysicsPlayerController>().ApplyLocalForce(reflectedVector * TrampolinePower);
                _audioManager.PlaySFX("bounce");
            }
        }
    }
}
