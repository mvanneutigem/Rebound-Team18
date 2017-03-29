using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private float TrampolinePower = 10;
    //pass up direction of trampoline as jumpvector

    void Update()
    {
        Debug.DrawRay(this.transform.position, this.transform.up * 5.0f, Color.red);
    }

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
