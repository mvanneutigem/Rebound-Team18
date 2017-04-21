using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityNew : MonoBehaviour {

    public enum Gravity_Direction{
            down,up,left,right,front,back
    }
    public Gravity_Direction GravityDirection;

    public Vector3 ChangeForwardVector = new Vector3(0, 0, 1);

    private PhysicsPlayerController _playerController;
    private Transform _playerTransform;

    public float ChangeDirectionSpeed = 20.0f;
    private bool _entered = false;

    private Vector3 _gravity;

    private float _angleUp = 0;

    private LayerMask mask;

    // Use this for initialization
    void Start () {
        var playerGameObject = GameObject.FindWithTag("Player");
        if (playerGameObject != null)
        {
            _playerTransform = playerGameObject.transform;
            _playerController = playerGameObject.GetComponent<PhysicsPlayerController>();
        }
        mask = LayerMask.GetMask("Gravity Portal");
    }

    // Update is called once per frame
    void Update()
    {

        bool rayFound = Physics.Raycast(_playerTransform.position, _playerController.GetForwardDir(), 5f, mask.value);
        if (rayFound)
        {
            _entered = false;
        }

        if (_entered)
        {
            // Set one of the local's axis's to the gravity direction
            // Check all axis's manually
            switch (GravityDirection)
            {
                case Gravity_Direction.up:
                    _gravity = -transform.up;
                    break;
                case Gravity_Direction.down:
                    _gravity = transform.up;
                    break;
                case Gravity_Direction.left:
                    _gravity = transform.right;
                    break;
                case Gravity_Direction.right:
                    _gravity = -transform.right;
                    break;
                case Gravity_Direction.front:
                    _gravity = -transform.forward;
                    break;
                case Gravity_Direction.back:
                    _gravity = transform.forward;
                    break;
                default:
                    break;
            }
            Debug.Log("Gravity = " + -_gravity);

            //Local Variables
            Vector3 up = Vector3.zero;
            Vector3 upSecond = Vector3.zero;
            Vector3 forward = Vector3.zero;


            // ****** FORWARD *******
            if (_playerController.GetForwardDir() != ChangeForwardVector)
            {
                forward = Vector3.RotateTowards(_playerController.GetForwardDir(), ChangeForwardVector, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
                transform.forward = _playerController.GetForwardDir();

                forward.Normalize();
                _playerController.SetForwardDir(forward);
            }

            // ****** UP *******
            _angleUp = Vector3.Angle(_playerController.GetUpVector(), _gravity);
            // If Angle is bigger then 90 (meaning it's a 180 turn) it rotates around it's right vector. resulting in every 180 turn to be CW
            // Else just 
            if (_angleUp > 90)
            {
                up = Vector3.RotateTowards(_playerController.GetUpVector(), transform.parent.transform.right, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
            }
            else
            {
                up = Vector3.RotateTowards(_playerController.GetUpVector(), _gravity, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
            }

            Debug.DrawRay(transform.position, up * 10, Color.green);

            up.Normalize();
            _playerController.SetUpVector(up);
        }
        //Debug.DrawRay(_transSelf.position, -GravityDirectionVector * 10, Color.yellow);
        //Debug.DrawRay(transform.position, localRight * 100, Color.yellow);
        Debug.DrawRay(transform.position, transform.right * 100, Color.yellow);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _entered = true;
        }
    }
}
