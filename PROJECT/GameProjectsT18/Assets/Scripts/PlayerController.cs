using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    //PROPERTIES
    public float MaxForwardSpeed = 15.0f;
    public float MaxLateralSpeed = 10.0f;
    //FIELDS
    [Range(0.0f, 1.0f)]
    public float RotateSpeed = 0.5f;
    public float ForwardDeceleration = 5.0f;
    public float LateralDeceleration = 10.0f;
    public float ForwardAcceleration = 5.0f;
    public float LateralAcceleration = 5.0f;
    private float _forwardVelocity = 0.0f;
    private float _lateralVelocity = 0.0f;

    private Vector3 _moveVector;
    private Vector3 _gravityVelocity;
    private Vector3 _horizontalVelocity;
    private Vector3 _worldSpaceVector;
    private Vector3 _moveDirForward = new Vector3(0, 0, 1);
    private Vector3 _moveDirRight;
    private CharacterController _characterController;
    public float JumpSpeed = 2.0f;
    private bool _jumping = false;
    private float dragForce;
    private Vector3 _upVector3;

    private Vector3 _camForward;

    //METHODS
    void Awake()
    {
        _upVector3 = new Vector3(0, 1, 0);
        _characterController = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        // input
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        //set the speed
        //rotate character in the direction it's moving in

        Vector3 acceleration = Vector3.zero;
        _moveDirRight = Vector3.Cross(_moveDirForward.normalized, _upVector3.normalized);

        if (Mathf.Abs(hInput) + Mathf.Abs(vInput) > float.Epsilon)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveVector.normalized);
            transform.rotation = targetRotation;

            // MOVEMENT
            acceleration.z += vInput * ForwardAcceleration * Time.deltaTime;
            acceleration.x += -hInput * LateralAcceleration * Time.deltaTime;

            _horizontalVelocity += acceleration;

            Vector3 forwardVel = (_horizontalVelocity + _moveDirForward) / 2.0f;

            if (Mathf.Abs(_horizontalVelocity.z) > MaxForwardSpeed)
            {
                _horizontalVelocity.z = (_horizontalVelocity.z > 0 ? MaxForwardSpeed : -MaxForwardSpeed);
            }

            if (Mathf.Abs(_horizontalVelocity.x) > MaxLateralSpeed)
            {
                _horizontalVelocity.x = (_horizontalVelocity.x > 0 ? MaxLateralSpeed : -MaxLateralSpeed);
            }
        }
        else if (Mathf.Abs(_horizontalVelocity.x) + Mathf.Abs(_horizontalVelocity.z) > 0.1f)
        {
            _horizontalVelocity.x += (_horizontalVelocity.x < 0 ? +LateralDeceleration * Time.deltaTime : -LateralDeceleration * Time.deltaTime);
            _horizontalVelocity.z += (_horizontalVelocity.z > 0 ? -ForwardDeceleration * Time.deltaTime : ForwardDeceleration * Time.deltaTime);
        }
        else
        {
            //to make it stop sliding randomly after no input for some time
            _horizontalVelocity.x = 0;
            _horizontalVelocity.z = 0;
        }


        if (Input.GetAxis("Jump") > 0 && !_jumping)
        {
            _horizontalVelocity.y += JumpSpeed;
            _jumping = true;
        }
        else
        {
            if (_characterController.isGrounded)
            {
                if (!_jumping)
                {
                    _horizontalVelocity.y = 0;
                }

                _jumping = false;
            }
        }

        //Gravity
        _horizontalVelocity += Physics.gravity * Time.deltaTime;

        //from local to worlspace
        _worldSpaceVector = _horizontalVelocity.z * _moveDirForward + _horizontalVelocity.x * _moveDirRight + _horizontalVelocity.y * _upVector3;
        _moveVector = _worldSpaceVector;

        //Move
        _characterController.Move((_moveVector) * Time.deltaTime);
    }

    public void SetForwardDir(Vector3 forward)
    {
        _camForward = forward;
        _camForward.y = 0;
        _camForward.Normalize();
    }

    public void ApplyForce(Vector3 force)
    {
        //_moveVector = force;
        _horizontalVelocity = force.z* _moveDirForward + force.x * _moveDirRight + force.y * _upVector3;
        ////from world to local space
        //_horizontalVelocity += force.x * _moveDirForward + force.z * _moveDirRight;
        //_moveVector += Physics.gravity * Time.deltaTime;
        //_characterController.Move(_moveVector * Time.deltaTime);
        //_horizontalVelocity.y += 10.0f;
        _jumping = true;
    }

    public void SetUpVector(Vector3 up)
    {
        _upVector3 = up;
    }
}

