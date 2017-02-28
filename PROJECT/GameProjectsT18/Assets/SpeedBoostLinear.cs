using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostLinear : MonoBehaviour {

    public float Distance;
    public float BoostTime;
    private PlayerController _player;
    private Transform _playerTransform;
    private float _timer = 0;
    private bool _enabled = false;
    private Vector3 _startPos;
    private Vector3 _endPos;
    void Update()
    {
        if (_enabled)
        {
            _timer += Time.deltaTime;

            if (_timer >= BoostTime)
            {
                _timer = 0;
                _enabled = false;
                _player.SetLockMovement(false);
                return;
            }

            _playerTransform.position = Vector3.Lerp(_startPos, _endPos, _timer / BoostTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _player = other.GetComponent<PlayerController>();
            _playerTransform = other.transform;
            _startPos = _playerTransform.position;
            Vector3 forward = _playerTransform.forward;
            forward.y = 0;
            forward.Normalize();
            _endPos = _startPos + forward * Distance;
            _player.SetLockMovement(true);

            _enabled = true;
        }
    }
}
