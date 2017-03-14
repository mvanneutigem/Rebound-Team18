﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    private Vector3 _velocity;
    private Vector3 _worldSpaceVector;
    private Vector3 _moveDirForward = new Vector3(0, 0, 1);
    private Vector3 _moveDirRight;
    private CharacterController _characterController;
    public float JumpSpeed = 2.0f;
    public float SlamSpeed = -30.0f;
    private bool _jumping = false;
    private float dragForce;
    private Vector3 _upVector3;
    private bool _movementLock = false;
    public float MaxFallForce = 50.0f;

    private Vector3 _camForward;

    //METHODS
    void Awake()
    {
        _upVector3 = new Vector3(0, 1, 0);
        _velocity = Vector3.zero;
        _characterController = this.GetComponent<CharacterController>();
        PlayerPrefs.SetInt("Score", 0);
    }

    void Update()
    {
        if (_velocity.y < -MaxFallForce) // Set to 70 for Tommie's Level; Original is 50
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        }

        if (!_movementLock)
        {

            // input
            float hInput = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Controller X");
            float vInput = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Controller Y");

            //set the speed
            //rotate character in the direction it's moving in

            Vector3 acceleration = Vector3.zero;
            _moveDirRight = Vector3.Cross(_moveDirForward.normalized, _upVector3.normalized);

            Quaternion targetRotation = Quaternion.LookRotation(_moveVector.normalized);
            transform.rotation = targetRotation;

            if (Mathf.Abs(hInput) > float.Epsilon)
            {
                if (_velocity.x < MaxLateralSpeed && hInput < 0)
                {
                    acceleration.x += -hInput * LateralAcceleration * Time.deltaTime;
                }

                if (_velocity.x > -MaxLateralSpeed && hInput > 0)
                {
                    acceleration.x += -hInput * LateralAcceleration * Time.deltaTime;
                }
            }
            else if (Mathf.Abs(_velocity.x) > 0.1f)
            {
                _velocity.x += (_velocity.x < 0 ? +LateralDeceleration * Time.deltaTime : -LateralDeceleration * Time.deltaTime);
            }

            if (Mathf.Abs(vInput) > float.Epsilon)
            {
                if (_velocity.z < MaxForwardSpeed && vInput > 0)
                {
                    acceleration.z += vInput * ForwardAcceleration * Time.deltaTime;
                }
            }
            else if (Mathf.Abs(_velocity.z) > 0.1f)
            {
                _velocity.z += (_velocity.z > 0 ? -ForwardDeceleration * Time.deltaTime : ForwardDeceleration * Time.deltaTime);
            }

            _velocity += acceleration;

            //jump
            if (Input.GetAxis("Jump") > 0 && !_jumping)
            {
                _velocity.y += JumpSpeed;
                _jumping = true;
            }
            else
            {
                if (_characterController.isGrounded)
                {
                    if (!_jumping)
                    {
                        _velocity.y = 0;
                    }

                    _jumping = false;
                }
            }

            if (transform.up != _upVector3)
            {
                var temp = transform.up;
                temp = Vector3.RotateTowards(temp, _upVector3, Mathf.PI, Mathf.PI);
                transform.up = temp;

                if (Vector3.Dot(transform.forward, _moveDirForward) < 0)
                {
                    transform.forward = _moveDirForward;
                }
            }

            //slam
            if (Input.GetButtonDown("Slam") && !_characterController.isGrounded)
            {
                _velocity.y += SlamSpeed;
            }

            //Gravity
            _velocity += Physics.gravity * Time.deltaTime;

            //from local to worlspace
            _worldSpaceVector = _velocity.z * _moveDirForward + _velocity.x * _moveDirRight + _velocity.y * _upVector3;
            _moveVector = _worldSpaceVector;

            //Move
            _characterController.Move((_moveVector) * Time.deltaTime);
        }
    }

    public void SetForwardDir(Vector3 forward)
    {

        _moveDirForward = forward;
    }
    public Vector3 GetForwardDir()
    {
        return _moveDirForward;
    }

    public void ApplyForce(Vector3 force)
    {
        _velocity.z = force.z;
        _velocity.x = force.x;
        _velocity.y = force.y;

        _jumping = true;
    }

    public void SetUpVector(Vector3 up)
    {
        _upVector3 = up;
    }

    public Vector3 GetUpVector()
    {
        return _upVector3;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }
    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }
    public void SetLockMovement(bool locked)
    {
        _movementLock = locked;
    }
    public bool GetLockMovement()
    {
        return _movementLock;
    }

    private Vector3 ToWorlSpace(Vector3 v)
    {
        return v.z * _moveDirForward + v.x * _moveDirRight + v.y * _upVector3;
    }
    public Vector3 GetWorldSpaceVelocity()
    {
        return ToWorlSpace(_velocity);
    }
    public Vector3 GetRightVector()
    {
        return _moveDirRight;
    }
}

