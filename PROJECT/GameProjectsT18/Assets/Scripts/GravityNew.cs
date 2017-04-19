using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityNew : MonoBehaviour {


    public Vector3 GravityDirectionVector = new Vector3(0, -1, 0);
    public Vector3 ChangeForwardVector = new Vector3(0, 0, 1);
    private PhysicsPlayerController _playerController;
    private Transform _playerTransform;
    private Transform _transSelf;
    public float RotateDistance = 20.0f;
    public float ChangeDirectionSpeed = 20.0f;
    private bool _entered = false;
    private Vector3 _playerStartUpVector;
    private Vector3 _playerStartForwardVector;
    private Vector3 _playerStartRightVector;
    private float _step = 0;
    private float _timer = 0;
    private float _angle = 0;
    private float _switchAngle = 0;
    private bool _hasBeenEntered = false;
    private LayerMask mask;

    // Use this for initialization
    void Start () {
        _transSelf = this.transform;
        var playerGameObject = GameObject.FindWithTag("Player");
        if (playerGameObject != null)
        {
            _playerTransform = playerGameObject.transform;
            _playerController = playerGameObject.GetComponent<PhysicsPlayerController>();
        }
        mask = LayerMask.GetMask("Gravity Portal");
    }
	
	// Update is called once per frame
	void Update () {

        bool rayFound = Physics.Raycast(_playerTransform.position, _playerController.GetForwardDir(), 200f, mask.value);
        if (rayFound)
        {
            _entered = false;
        }
        if (_entered)
        {
            //Local Variable
            Vector3 up = _playerController.GetUpVector();
            //Vector3 upSecond = _playerController.GetUpVector();
            Vector3 forward = _playerController.GetForwardDir();

            forward = Vector3.RotateTowards(forward, ChangeForwardVector, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
            up = Vector3.RotateTowards(up, -GravityDirectionVector, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
            //upSecond = Vector3.Cross(_playerController.GetForwardDir(), _playerController.GetRightVector());

            //up = up + upSecond;
            up.Normalize();
            forward.Normalize();
            _playerController.SetForwardDir(forward);
            _playerController.SetUpVector(up);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            _entered = true;
        }
    }
}
