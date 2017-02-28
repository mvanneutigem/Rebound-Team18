using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private float _rotSpeed = 15.0f;
    private bool _isTriggerEntered = false;
    private float _coreSpeed = 100.0f;
    private float _distance = 2.0f;
    private GameObject _player;
    private bool _setNewPos = true;

    private Transform transformPosition;

    void Start()
    {
        transformPosition = transform;
    }
    void Update()
    {
        //rotation
        transform.Rotate(Vector3.up, _rotSpeed * Time.deltaTime);

        //if collected it follows
        if (_isTriggerEntered == true)
        {
            //new script:
            //transformPosition = transform;
            if (_setNewPos)
            {
                var newPos = _player.transform.position;
                newPos.z -= _distance;
                transformPosition.position = newPos;
                _setNewPos = false;
            }
            var temp = transformPosition.position;
            temp.y = _player.transform.position.y;
            transformPosition.position = temp;
            var curDistance = _player.transform.position - transformPosition.position;
            transformPosition.RotateAround(_player.transform.position, _player.GetComponent<PlayerController>().GetUpVector(), _coreSpeed * Time.deltaTime);

            if (curDistance.magnitude > _distance)
            {
                curDistance.Normalize();
                curDistance *= _distance;
                transformPosition.position = Vector3.Lerp(transformPosition.position, _player.transform.position - curDistance, 0.5f);
            }

            transform.position = transformPosition.position;

            //old script:
            //Vector3 dir = PlayerTransform.position - transform.position;
            ////direction
            //dir.Normalize();

            //transform.position += dir * _coreSpeed * Time.deltaTime;

            ////transform.LookAt(PlayerTransform);
            ////transform.position += transform.forward * 10 * Time.deltaTime;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _isTriggerEntered = true;
            _player = other.gameObject;
        }
            
    }
}
