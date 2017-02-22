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

    private Vector3 _moveVector;
    private Vector3 _horizontalVelocity;
    private Vector3 _worldSpaceHorizontal;
    private Vector3 _moveDirForward;
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
        _characterController = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        // input
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        //set the speed
        //rotate character in the direction it's moving in

        Vector3 tempMove = Vector3.zero;
        _moveDirForward = _camForward;
        _moveDirRight = new Vector3(_moveDirForward.z, _moveDirForward.y, -_moveDirForward.x);

        if (Mathf.Abs(hInput) + Mathf.Abs(vInput) > float.Epsilon)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_camForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RotateSpeed);

            // MOVEMENT
            tempMove.x = vInput * ForwardAcceleration * Time.deltaTime;
            tempMove.z = hInput * LateralAcceleration * Time.deltaTime;

            _horizontalVelocity.x += tempMove.x;
            _horizontalVelocity.z += tempMove.z;

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
            _horizontalVelocity.x += (_horizontalVelocity.x > 0 ? -LateralDeceleration * Time.deltaTime : LateralDeceleration * Time.deltaTime);
            _horizontalVelocity.z += (_horizontalVelocity.z > 0 ? -ForwardDeceleration * Time.deltaTime : ForwardDeceleration * Time.deltaTime);
        }
        else
        {
            //to make it stop sliding randomly after no input for some time
            _horizontalVelocity.x = 0;
            _horizontalVelocity.z = 0;
        }
        //from local to worlspace
        _worldSpaceHorizontal = _horizontalVelocity.x * _moveDirForward + _horizontalVelocity.z * _moveDirRight;

        _moveVector.x = _worldSpaceHorizontal.x;
        _moveVector.z = _worldSpaceHorizontal.z;

        if (Input.GetAxis("Jump") > 0 && !_jumping)
        {
            _moveVector = Vector3.up * JumpSpeed;
            _jumping = true;
        }
        else
        {
            if (_characterController.isGrounded)
            {
                _jumping = false;
            }
        }

        //Gravity
        _moveVector += Physics.gravity * Time.deltaTime;

        //Move
        _characterController.Move(_moveVector * Time.deltaTime);
    }

    public void SetForwardDir(Vector3 forward)
    {
        _camForward = forward;
        _camForward.y = 0;
        _camForward.Normalize();
    }

    public void ApplyForce(Vector3 force)
    {
        _moveVector = force;
        //from world to local space
        _moveDirRight = new Vector3(_moveDirForward.z, _moveDirForward.y, -_moveDirForward.x);
        _horizontalVelocity += force.x * _camForward + force.z * _moveDirRight;
        _moveVector += Physics.gravity * Time.deltaTime;
        _characterController.Move(_moveVector * Time.deltaTime);
    }

    public void SetUpVector(Vector3 up)
    {
        _upVector3 = up;
    }
}

