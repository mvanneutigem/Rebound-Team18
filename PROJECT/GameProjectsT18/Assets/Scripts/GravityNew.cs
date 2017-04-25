using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityNew : MonoBehaviour {
    public enum Forward_Direction
    {
        front, back, left, right, up, down
    }
    public Forward_Direction ForwardDirection;

    public enum Gravity_Direction
    {
        down, up, left, right, front, back
    }
    public Gravity_Direction GravityDirection;


    //public Vector3 ChangeGravityDirectionInWorld = new Vector3(0, -1, 0);
    //public Vector3 ChangeForwardInWorld = new Vector3(0, 0, 1);

    private PhysicsPlayerController _playerController;
    private Transform _playerTransform;

    public float ChangeDirectionSpeed = 20.0f;
    private bool _entered = false;

    private Vector3 _newUpVector;
    private Vector3 _newForwardVector;

    private float _angleUp = 0;
    private float _angleForward = 0;

    private LayerMask mask;

    private GameObject _pseudo;
    private Rewind _rewindScript;

    // Use this for initialization
    void Start () {

        var playerGameObject = GameObject.FindWithTag("Player");
        if (playerGameObject != null)
        {
            _playerTransform = playerGameObject.transform;
            _playerController = playerGameObject.GetComponent<PhysicsPlayerController>();
            _rewindScript = playerGameObject.GetComponent<Rewind>();
        }
        mask = LayerMask.GetMask("Gravity Portal");

        _pseudo = new GameObject("pseudo portal");
        _pseudo.transform.up = gameObject.transform.up;
        _pseudo.transform.right = gameObject.transform.right;
        _pseudo.transform.forward = gameObject.transform.forward;
        _pseudo.transform.rotation = gameObject.transform.rotation;
        _pseudo.transform.position = gameObject.transform.position;
        _pseudo.transform.localScale = gameObject.transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("new Forward: " +_newForwardVector);

        //bool rayFound = Physics.Raycast(_playerTransform.position, _playerController.GetForwardDir(), 5f, mask.value);
        //if (rayFound)
        //{
        //    _entered = false;
        //}

        if (_entered)
        {
            if (_playerController.GetLockMovement())
            {
                _entered = false;
            }

            //Debug.Log("new Forward in Gravity: " + _newForwardVector);
            //Debug.Log("new Up in Gravity: " + _newUpVector);
            _rewindScript.SetNewForward(_newForwardVector);
            _rewindScript.SetNewUp(_newUpVector);
            _rewindScript.SetRotationSpeed(ChangeDirectionSpeed);
            _rewindScript.SetPseudoRight(_pseudo.transform.right);

            //Local Variables
            Vector3 up = _playerController.GetUpVector();
            Vector3 forward = _playerController.GetForwardDir();


            // ****** FORWARD *******
            _angleForward = Vector3.Angle(_playerController.GetForwardDir(), _newForwardVector);
            //Debug.Log("current forward: " + _playerController.GetForwardDir());
            //Debug.Log("new forward: " + _newForwardVector);
            //Debug.Log("Angle PlayerForward/NewForward: " + _angleForward);
            // Allow for some mistakes in this angle since this angle is only set upon entering and isn't updated every frame
            if (_angleForward > 95)
            {
                forward = Vector3.RotateTowards(_playerController.GetForwardDir(), -_pseudo.transform.right, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
            }
            else
            {
                forward = Vector3.RotateTowards(_playerController.GetForwardDir(), _newForwardVector, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
            }

            forward.Normalize();
            _playerController.SetForwardDir(forward);
            if (_angleForward > 0)
            {
                _pseudo.transform.forward = _playerController.GetForwardDir();
            }


            // ****** UP *******
            _angleUp = Vector3.Angle(_playerController.GetUpVector(), _newUpVector);
            //Debug.Log("current Up: " + _playerController.GetUpVector());
            //Debug.Log("new Up: " + _newUpVector);
            //Debug.Log("Angle PlayerUp/NewUp: " + _angleUp);

            // If Angle is bigger then 90 (meaning it's a 180 turn) it rotates around it's right vector. resulting in every 180 turn to be CW
            //Debug.Log("Pseudo right in Gravity: " + _pseudo.transform.right);
            // Else just 
            if (_angleUp > 95)
            {
                up = Vector3.RotateTowards(_playerController.GetUpVector(), _pseudo.transform.right, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
            }
            else
            {
                up = Vector3.RotateTowards(_playerController.GetUpVector(), _newUpVector, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
            }

            Debug.DrawRay(transform.position, up * 10, Color.green);

            up.Normalize();
            _playerController.SetUpVector(up);
        }

        Debug.DrawRay(transform.position, _pseudo.transform.forward * 100, Color.yellow);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (ForwardDirection)
            {
                case Forward_Direction.front:
                    _newForwardVector = new Vector3(0, 0, 1);
                    break;
                case Forward_Direction.back:
                    _newForwardVector = new Vector3(0, 0, -1);
                    break;
                case Forward_Direction.left:
                    _newForwardVector = new Vector3(-1, 0, 0);
                    break;
                case Forward_Direction.right:
                    _newForwardVector = new Vector3(1, 0, 0);
                    break;
                case Forward_Direction.up:
                    _newForwardVector = new Vector3(0, 1, 0);
                    break;
                case Forward_Direction.down:
                    _newForwardVector = new Vector3(0, -1, 0);
                    break;
                default:
                    break;
            }

            // Set one of the local's axis's to the gravity direction
            // Check all axis's manually
            switch (GravityDirection)
            {
                case Gravity_Direction.up:
                    _newUpVector = new Vector3(0, -1, 0);
                    break;
                case Gravity_Direction.down:
                    _newUpVector = new Vector3(0, 1, 0);
                    break;
                case Gravity_Direction.left:
                    _newUpVector = new Vector3(1, 0, 0);
                    break;
                case Gravity_Direction.right:
                    _newUpVector = new Vector3(-1, 0, 0);
                    break;
                case Gravity_Direction.front:
                    _newUpVector = new Vector3(0, 0, -1);
                    break;
                case Gravity_Direction.back:
                    _newUpVector = new Vector3(0, 0, 1);
                    break;
                default:
                    break;
            }

            _entered = true;
        }
    }

    public void SetNewForwardVector(Vector3 vec)
    {
        _newForwardVector = vec;
    }
    public void SetNewUpVector (Vector3 vec)
    {
        _newUpVector = vec;
    }

    public void SetEntered(bool yup)
    {
        _entered = yup;
    }

    public void SetRotationSpeed(float value)
    {
        ChangeDirectionSpeed = value;
    }

    public void SetPseudoRight(Vector3 right)
    {
        _pseudo.transform.right = right;
    }
}

