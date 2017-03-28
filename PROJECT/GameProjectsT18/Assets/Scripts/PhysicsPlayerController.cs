using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PhysicsPlayerController : MonoBehaviour
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
    private Rigidbody _playerRigidBody;
    private Transform _transSelf;
    public float JumpSpeed = 2.0f;
    public float SlamSpeed = -30.0f;
    private bool _jumping = false;
    private float dragForce;
    private Vector3 _upVector3;
    private bool _movementLock = false;
    public float MaxFallForce = 50.0f;

    private int _grounded = 0;
    private Vector3 _lastSurfaceNormal;

    private Vector3 _camForward;
    private Animator myAnimator;
    private bool OnTrampoline = false;

    //METHODS
    void Awake()
    {
        _transSelf = this.transform;
        _upVector3 = new Vector3(0, 1, 0);
        _velocity = Vector3.zero;
        _playerRigidBody = this.GetComponent<Rigidbody>();
        PlayerPrefs.SetInt("Score", 0);
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        _velocity = _playerRigidBody.velocity;
        if (_velocity.y < -MaxFallForce) // Will change into Death planes 
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

            Vector3 force = Vector3.zero;
            _moveDirRight = Vector3.Cross( _upVector3.normalized, _moveDirForward.normalized);

            if (Mathf.Abs(hInput) > float.Epsilon)
            {
                if ( _velocity.x < MaxLateralSpeed && hInput > 0)
                {
                    force.x += hInput * LateralAcceleration * Time.deltaTime;
                }

                if ( _velocity.x > -MaxLateralSpeed && hInput < 0)
                {
                    force.x += hInput * LateralAcceleration * Time.deltaTime;
                }
            }
            else
            {
                //decceleration
                if (_velocity.x > 0)
                {
                    force.x -= LateralDeceleration * Time.deltaTime;
                }
                if (_velocity.x < 0)
                {
                    force.x += LateralDeceleration * Time.deltaTime;
                }
            }

            if (Mathf.Abs(vInput) > float.Epsilon)
            {
                if ( _velocity.z > -MaxForwardSpeed && vInput < 0)
                {
                    force.z += vInput * ForwardAcceleration * Time.deltaTime;
                }

                if ( _velocity.z < MaxForwardSpeed && vInput > 0)
                {
                    force.z += vInput * ForwardAcceleration * Time.deltaTime;
                }
            }
            else
            {
                //decceleration
                if (_velocity.z > 0)
                {
                    force.z -= LateralDeceleration * Time.deltaTime;
                }
                if (_velocity.z < 0)
                {
                    force.z += LateralDeceleration * Time.deltaTime;
                }
            }
            //jump
            if (Input.GetAxis("Jump") > 0 && !_jumping)
            {
                _playerRigidBody.AddForce(_lastSurfaceNormal * JumpSpeed, ForceMode.Impulse);
                _jumping = true;
            }
            else
            {
                if (_grounded > 0)
                {
                    _jumping = false;
                }
            }

            //slam
            if (Input.GetButtonDown("Slam") )
            {
                _playerRigidBody.AddForce(_lastSurfaceNormal * SlamSpeed, ForceMode.Impulse);
            }

            //from local to worlspace
            Vector3 gravity = _upVector3;
            gravity *= -9.81f;
            _playerRigidBody.velocity += gravity * Time.deltaTime;
            force = ToWorldSpace(force);

            //Move
            Debug.Log("up " + _upVector3);
            Debug.Log("force " + force);
            Debug.Log("velocity " + _velocity);
            _playerRigidBody.AddForce(force * 50.0f);
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

    public void ApplyForce(Vector3 appliedForce)
    {
        _playerRigidBody.velocity = appliedForce;
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
        return _playerRigidBody.velocity;
    }
    public void SetVelocity(Vector3 velocity)
    {
        _playerRigidBody.velocity = velocity; 
    }
    public void SetLockMovement(bool locked)
    {
        _movementLock = locked;
    }
    public bool GetLockMovement()
    {
        return _movementLock;
    }

    private Vector3 ToWorldSpace(Vector3 v)
    {
        return v.z * _moveDirForward + v.x * _moveDirRight + v.y * _upVector3;
    }
    public Vector3 GetWorldSpaceVelocity()
    {
        return ToWorldSpace(_velocity);
    }
    public Vector3 GetRightVector()
    {
        return _moveDirRight;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TrampolineBox")
        {
            OnTrampoline = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrampolineBox")
        {
            OnTrampoline = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        _grounded += 1;
        Vector3 point = other.contacts[0].point;
        Vector3 dir = _transSelf.position - point;
        _lastSurfaceNormal = dir.normalized;
    }

    void OnCollisionExit(Collision other)
    {
        _grounded -= 1;
    }
}

