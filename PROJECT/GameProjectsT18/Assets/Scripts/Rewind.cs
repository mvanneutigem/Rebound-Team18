using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour {

    private ArrayList _previousPositions = new ArrayList();
    private ArrayList _previousRotations = new ArrayList();
    private ArrayList _previousVelocities = new ArrayList();
    private ArrayList _previousUpVectors = new ArrayList();
    private int _arrayIdx = 0;
    private GameObject _playerGameObject;
    private PlayerController _playerController;
    private bool bRewinding = false;
    public int RewindDuration; // for later use once a timer is available
    private Vector3 _velocity;

	void Start ()
    {
        _playerGameObject = GameObject.FindWithTag("Player");
        _playerController = _playerGameObject.GetComponent<PlayerController>();
    }
	
	void Update ()
    {
        if(_arrayIdx > _previousPositions.Count - 1 | _arrayIdx < 0)
        {
            _arrayIdx = _previousPositions.Count;
        }

		if(!bRewinding)
        {
            Transform playerTransform = _playerGameObject.GetComponent<Transform>();
            Debug.Log("Player Position:" + playerTransform.position
                + "Player Rotation:" + playerTransform.localRotation
                + "Position Array:" + _previousPositions.Count 
                + "Position Idx" + _arrayIdx);
            // Save Pos and Rot
            _previousPositions.Add(playerTransform.position);
            _previousRotations.Add(playerTransform.localRotation);
            _previousVelocities.Add(_playerController.GetVelocity());
            _previousUpVectors.Add(_playerController.GetUpVector());
            _arrayIdx++;
        }
        else { RewindTime(); }


        if(Input.GetKeyDown("r"))
        {
            bRewinding = true;
            _playerController.SetLockMovement(true);
        } 
        if(Input.GetKeyUp("r"))
        {
            bRewinding = false;
            _playerController.SetLockMovement(false);
            _playerController.SetVelocity((Vector3)_previousVelocities[_arrayIdx-1]);
        }
    }
    void RewindTime()
    {
        _arrayIdx--;
        if(_arrayIdx > 0)
        {
            Transform playerTransform = _playerGameObject.GetComponent<Transform>();
            playerTransform.position = (Vector3)_previousPositions[_arrayIdx-1];
            playerTransform.localRotation = (Quaternion)_previousRotations[_arrayIdx-1];
            _playerController.SetUpVector((Vector3)_previousUpVectors[_arrayIdx-1]);

            _previousPositions.RemoveAt(_arrayIdx);
            _previousRotations.RemoveAt(_arrayIdx);
            _previousVelocities.RemoveAt(_arrayIdx);
            _previousUpVectors.RemoveAt(_arrayIdx);

            Debug.Log("Player Position:" + playerTransform.position + "Position Array:" + _previousPositions.Count + "Position Idx" + _arrayIdx);
        }
    }
}
