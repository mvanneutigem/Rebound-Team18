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
    private Vector2 _horizontalVelocity;
    private Vector3 _moveDirForward;
    private Vector3 _moveDirRight;
    private CharacterController _characterController;
    public float JumpSpeed = 2.0f;
    private bool _jumping = false;
    private float dragForce;
    private Vector3 _trampolineInfluence = Vector3.zero;
    public float TrampolineUpPower = 0f;

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

        if (Mathf.Abs(hInput) + Mathf.Abs(vInput) != 0)
        {
            _moveDirForward = _camForward;
            _moveDirRight = new Vector3(_moveDirForward.z, _moveDirForward.y, -_moveDirForward.x);

            Quaternion targetRotation = Quaternion.LookRotation(_camForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RotateSpeed);


            // MOVEMENT
            tempMove = (vInput * _moveDirForward * ForwardAcceleration * Time.deltaTime)
                 + (hInput * _moveDirRight * LateralAcceleration * Time.deltaTime);

            _horizontalVelocity.x += tempMove.x;
            _horizontalVelocity.y += tempMove.z;

            if (Mathf.Abs(_horizontalVelocity.y) > MaxForwardSpeed)
            {
                _horizontalVelocity.y = (_horizontalVelocity.y > 0 ? MaxForwardSpeed : -MaxForwardSpeed);
            }

            if (Mathf.Abs(_horizontalVelocity.x) > MaxLateralSpeed)
            {
                _horizontalVelocity.x = (_horizontalVelocity.x > 0 ? MaxLateralSpeed : -MaxLateralSpeed);
            }
        }
        else
        {
            _horizontalVelocity.x += (_horizontalVelocity.x > 0 ? -LateralDeceleration * Time.deltaTime : LateralDeceleration * Time.deltaTime);
            _horizontalVelocity.y += (_horizontalVelocity.y > 0 ? -ForwardDeceleration * Time.deltaTime : ForwardDeceleration * Time.deltaTime);
        }

        _moveVector.x = _horizontalVelocity.x;
        _moveVector.z = _horizontalVelocity.y;

        
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
                _trampolineInfluence = Vector3.zero;
            }
            else
            {
                //let player push back against trampoline force
                if (tempMove.x > 0 && _trampolineInfluence.x < 0)
                {
                    _trampolineInfluence.x += tempMove.x * Time.deltaTime;
                }
                if (tempMove.x < 0 && _trampolineInfluence.x > 0)
                {
                    _trampolineInfluence.x += tempMove.x * Time.deltaTime;
                }
                if (tempMove.z > 0 && _trampolineInfluence.z < 0)
                {
                    _trampolineInfluence.z += tempMove.z * Time.deltaTime;
                }
                if (tempMove.z < 0 && _trampolineInfluence.z > 0)
                {
                    _trampolineInfluence.z += tempMove.z * Time.deltaTime;
                }
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
        //used for trampolines
        _moveVector = force + Vector3.up * TrampolineUpPower;
        _trampolineInfluence = force;
        _trampolineInfluence.Normalize();
        _trampolineInfluence.y = 0;
        _characterController.Move(_moveVector * Time.deltaTime);
    }
}

