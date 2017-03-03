using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPortal : MonoBehaviour {
    public Vector3 GravityDirectionVector = new Vector3(-1, 0, 0);
    private PlayerController _playerController;
    public float RotateTime = 0.5f;
    private bool _entered = false;
    private bool _rewinding = false;
    private Vector3 _upVector3;
    private Vector3 _rightVector3;
    private float _step = 0;
    private float _timer = 0;
    private float _angle = 0;
    void Start()
    {
        var playerGameObject = GameObject.FindWithTag("Player");
        if (playerGameObject != null)
        {
            _upVector3 = playerGameObject.transform.up;
            _playerController = playerGameObject.GetComponent<PlayerController>();
        }
    }
    void Update()
    {
        if (_entered)
        {
            _rewinding = GameObject.FindWithTag("Player").GetComponent<Rewind>().getRewinding();
            if (!_rewinding)
            {
                _timer += Time.deltaTime;
                float fps = 1.0f / Time.deltaTime;
                _angle = Vector3.Angle(_upVector3, -GravityDirectionVector);

                if (_angle == 180)
                {
                    _upVector3 = Vector3.RotateTowards(_upVector3, _rightVector3, _step / fps, 0.0f);
                }
                else if (_angle > 0)
                {
                    _upVector3 = Vector3.RotateTowards(_upVector3, -GravityDirectionVector, _step / fps, 0.0f);
                }

                _upVector3.Normalize();
                _playerController.SetUpVector(_upVector3);
                Debug.Log("upvector : " + _upVector3);

                if (_timer >= RotateTime)
                {
                    _timer = 0;
                    _entered = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !_entered && !_playerController.GetLockMovement())
        {
            _upVector3 = _playerController.GetUpVector();
            _angle = Vector3.Angle(_upVector3, -GravityDirectionVector);
            _step = (Mathf.Deg2Rad * _angle) / RotateTime;
            _rightVector3 = _playerController.GetRightVector();
            _entered = true;
        }
    }
}
