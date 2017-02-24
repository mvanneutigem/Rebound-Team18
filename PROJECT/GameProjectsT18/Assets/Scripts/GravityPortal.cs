using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPortal : MonoBehaviour {
    public Vector3 GravityDirectionVector = new Vector3(-1, 0, 0);
    private PlayerController _playerController;
    public float RotateTime = 0.5f;
    private float _timer = 0; 
    private bool Entered = false;
    private Vector3 _upVector3;
    private Vector3 _rightVector3;
    private bool _180Turn = false;

    void Start()
    {
        //LevelGameObject = GameObject.FindWithTag("level");
        var playerGameObject = GameObject.FindWithTag("Player");
        if (playerGameObject != null)
        {
            _upVector3 = playerGameObject.transform.up;
            _playerController = playerGameObject.GetComponent<PlayerController>();
        }
    }
    void Update()
    {
        if (Entered)
        {
            _timer += Time.deltaTime;
            //Quaternion targetRotation = Quaternion.AngleAxis(rotation, rotationAxis);
            float angle = Vector3.Angle(_upVector3, -GravityDirectionVector);
            if (angle >= 180)
            {
                _upVector3 = Vector3.RotateTowards(_upVector3, _rightVector3, _timer / 2 / RotateTime, 0.0f);
                _180Turn = true;
            }
            else
            {
                if (_180Turn)
                {

                    _upVector3 = Vector3.RotateTowards(_upVector3, -GravityDirectionVector, _timer / 2 / RotateTime, 0.0f);
                }
                else
                {
                    _upVector3 = Vector3.RotateTowards(_upVector3, -GravityDirectionVector, _timer / RotateTime, 0.0f);
                }
            }
            _upVector3.Normalize();
            _playerController.SetUpVector(_upVector3);
            Debug.Log("upvector : " + _upVector3);
            //LevelGameObject.transform.rotation = Quaternion.Lerp(_startRotation, targetRotation, _timer / RotateTime);

            if (_timer >= RotateTime)
            {
                // disable self
                //this.GetComponent<GravityPortal>().enabled = false;
                Entered = false;
                _timer = 0;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !Entered && !_playerController.GetLockMovement())
        {
            Entered = true;
            _upVector3 = _playerController.GetUpVector();
            _rightVector3 = _playerController.GetRightVector();
        }
    }
}
