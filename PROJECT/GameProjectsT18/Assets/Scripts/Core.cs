using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private float _rotSpeed = 15.0f;
    private bool _isTriggerEntered = false;
    private float _coreSpeed = 40.0f;


    public Transform PlayerTransform;
    void Update()
    {
        //rotation
        transform.Rotate(Vector3.up, _rotSpeed * Time.deltaTime);

        //checking distance
        float distance = Vector3.Distance(transform.position, PlayerTransform.position);
        //if collected it follows
        if (_isTriggerEntered == true && distance > 2)
        {

            Vector3 dir = PlayerTransform.position - transform.position;
            //direction
            dir.Normalize();

            transform.position += dir * _coreSpeed * Time.deltaTime;

            //transform.LookAt(PlayerTransform);
            //transform.position += transform.forward * 10 * Time.deltaTime;
        }
    }
    void OnTriggerEnter()
    {
        _isTriggerEntered = true;
    }
}
