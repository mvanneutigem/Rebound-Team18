using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour {

    // Array Lists
    private ArrayList _previousPositions = new ArrayList();
    private ArrayList _previousRotations = new ArrayList();
    private ArrayList _previousVelocities = new ArrayList();

    private int _arrayIdx = 0;

    private GameObject _playerGameObject;
    private PlayerController _playerController;
    private Transform _playerTransform;

    private bool _rewinding = false;

	void Start ()
    {
        _playerGameObject = GameObject.FindWithTag("Player");
        _playerController = _playerGameObject.GetComponent<PlayerController>();
        _playerTransform = _playerGameObject.GetComponent<Transform>();
    }
	
	void Update ()
    {
        if (Time.timeScale > float.Epsilon)
        {
            if (_arrayIdx > _previousPositions.Count - 1 | _arrayIdx < 0)
            {
                _arrayIdx = _previousPositions.Count;
            }

            if (!_rewinding)
            {
                _previousPositions.Add(_playerTransform.position);
                _previousRotations.Add(_playerTransform.localRotation);
                _previousVelocities.Add(_playerController.GetVelocity());
                _arrayIdx++;
            }
            else { RewindTime(); }


            if (Input.GetAxisRaw("Rewind") > 0)
            {
                _rewinding = true;
                _playerController.SetLockMovement(true);
            }
            if (Input.GetAxisRaw("Rewind") == 0 && _rewinding)
            {
                _rewinding = false;
                _playerController.SetLockMovement(false);
                _playerController.SetVelocity((Vector3)_previousVelocities[_arrayIdx - 1]);
            }
        }
        
    }
    void RewindTime()
    {
        _arrayIdx--;
        if(_arrayIdx > 0)
        {
            _playerTransform.position = (Vector3)_previousPositions[_arrayIdx-1];
            _playerTransform.localRotation = (Quaternion)_previousRotations[_arrayIdx-1];

            _previousPositions.RemoveAt(_arrayIdx);
            _previousRotations.RemoveAt(_arrayIdx);
            _previousVelocities.RemoveAt(_arrayIdx);
        }
    }
    public bool getRewinding()
    {
        return _rewinding;
    }
}
