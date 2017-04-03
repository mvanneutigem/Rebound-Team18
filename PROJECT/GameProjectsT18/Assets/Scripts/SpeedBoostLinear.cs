using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostLinear : MonoBehaviour
{

    public float Distance;
    public float BoostTime;
    private PlayerController _player;
    private Transform _playerTransform;
    private float _timer = 0;
    private bool _enabled = false;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private GameObject _trailObj;
    private TrailRenderer _trail;
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
                Destroy(_trailObj);
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
            _playerTransform = _player.GetComponent<Transform>();
            _startPos = _playerTransform.position;
            Vector3 forward = _player.GetForwardDir();
            forward.y = 0;
            forward.Normalize();
            _endPos = _startPos + forward * Distance;
            _player.SetLockMovement(true);

            _trailObj = new GameObject();
            _trailObj.AddComponent<TrailRenderer>();
            _trail = _trailObj.GetComponent<TrailRenderer>();
            _trailObj.transform.SetParent(_playerTransform);
            _trailObj.transform.localPosition = Vector3.zero;
            _trail.widthMultiplier = 0.5f;
            _trail.enabled = true;
            _trail.sharedMaterial = new Material(Shader.Find("Standard"));
            _trail.sharedMaterial.EnableKeyword("_EMISSION");
            _trail.sharedMaterial.SetColor("_EmissionColor", Color.cyan);
            _trail.sharedMaterial.color = Color.cyan;
            _enabled = true;
        }
    }
}