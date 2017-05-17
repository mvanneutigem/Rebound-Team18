using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PhysicsPlayerController : MonoBehaviour
{
    //PROPERTIES
    public float MovementSnappiness = 1.0f;
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
    public float JumpSpeed = 10.0f;
    public float SlamSpeed = -30.0f;
    private bool _jumping = false;
    private float dragForce;
    private Vector3 _upVector3;
    private bool _movementLock = false;
    public float MaxFallForce = 50.0f;
    public bool KillPlayer = false;

    private int _grounded = 0;
    private Vector3 _lastSurfaceNormal;

    private Vector3 _camForward;
    private bool OnTrampoline = false;

    private const float SLAM_CD_TIME = 0.1f;
    private float _slamTimer = SLAM_CD_TIME;
    private Vector3 _prevVelocity = Vector3.zero;
    private PlayerDummy _dummy;
    public float MaxSquash = 0.7f;
    public float MaxStretch = 1.5f;
    private float _squashValue = 0;
    private Vector3 _squashDirection;

    //keys
    private string _slamKey;

    public enum Mat
    {
        METAL,
        RUBBER,
        GLASS,
        WOOD
    }
    public Mat materialstate;

    public void SetState(int id)
    {
        materialstate = (Mat)id;
    }


    //METHODS
    void Awake()
    {
        Application.targetFrameRate = 500;
        QualitySettings.vSyncCount = 0;

        materialstate = Mat.RUBBER;
        _transSelf = this.transform;
        _upVector3 = new Vector3(0, 1, 0);
        _velocity = Vector3.zero;
        _playerRigidBody = this.GetComponent<Rigidbody>();
        _playerRigidBody.maxAngularVelocity = 50;
        PlayerPrefs.SetInt("Score", 0);
        _slamKey = PlayerPrefs.GetString("Slam");
        _dummy = GameObject.Find("PlayerDummy").GetComponent<PlayerDummy>();
    }

    void Update()
    {
        _moveDirRight = Vector3.Cross(_upVector3.normalized, _moveDirForward.normalized);
        _velocity = ConvertToLocalSpace(_playerRigidBody.velocity);

        if (KillPlayer)
        {
            if (_velocity.y < -MaxFallForce) // Will change into Death planes 
            {
                int currentScene = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentScene);
            }
        }

        if (!_movementLock)
        {

            // input
            float hInput = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Controller X");
            float vInput = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Controller Y");

            //set the speed
            //rotate character in the direction it's moving in

            Vector3 force = Vector3.zero;

            if (Mathf.Abs(hInput) > float.Epsilon)
            {
                if (_velocity.x < MaxLateralSpeed && hInput > 0)
                {
                    force.x += hInput * LateralAcceleration * Time.deltaTime;
                }

                if (_velocity.x > -MaxLateralSpeed && hInput < 0)
                {
                    force.x += hInput * LateralAcceleration * Time.deltaTime;
                }
            }
            else
            {
                if (_grounded <= 0)
                {
                    //deceleration
                    if (_velocity.x > 0)
                    {
                        force.x -= LateralDeceleration * Time.deltaTime;
                    }
                    if (_velocity.x < 0)
                    {
                        force.x += LateralDeceleration * Time.deltaTime;
                    }
                }
            }

            if (Mathf.Abs(vInput) > float.Epsilon)
            {
                if (_velocity.z > -MaxForwardSpeed && vInput < 0)
                {
                    force.z += vInput * ForwardAcceleration * Time.deltaTime;
                }

                if (_velocity.z < MaxForwardSpeed && vInput > 0)
                {
                    force.z += vInput * ForwardAcceleration * Time.deltaTime;
                }
            }
            else
            {
                //deceleration
                if (_grounded <= 0 )
                {
                    if (_velocity.z > 0)
                    {
                        force.z -= ForwardDeceleration * Time.deltaTime;
                    }
                    if (_velocity.z < 0)
                    {
                        force.z += ForwardDeceleration * Time.deltaTime;
                    }
                }
            }
            if (_velocity.z > MaxForwardSpeed)
            {
                //deceleration at max
                if (_velocity.z > 0)
                {
                    force.z -= ForwardDeceleration * Time.deltaTime;
                }
                if (_velocity.z < 0)
                {
                    force.z += ForwardDeceleration * Time.deltaTime;
                }
            }

            //jump
            if ((Input.GetAxis("Jump") > 0 || Input.GetKeyDown(_slamKey)) && !_jumping )
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
            if (_slamTimer >= SLAM_CD_TIME)
            {
                if (Input.GetButtonDown("Slam") || Input.GetKeyDown(_slamKey))
                {
                    _squashValue = MaxStretch;
                    _slamTimer = 0.0f;
                    _playerRigidBody.AddForce(_upVector3 * SlamSpeed, ForceMode.Impulse);
                }
            }
            else
            {
                _slamTimer += Time.deltaTime;
            }

            //from local to worlspace
            Vector3 gravity = _upVector3;
            gravity *= -9.81f;
            _playerRigidBody.velocity += gravity * Time.deltaTime;
            force = ToWorldSpace(force);

            //Move
            //Debug.Log("up " + _upVector3);
            _playerRigidBody.AddForce(force * MovementSnappiness, ForceMode.VelocityChange);

            if (materialstate == Mat.RUBBER)
            {
                if (_squashValue > 0)
                {
                    if (_squashValue > MaxStretch)
                    {
                        _squashValue = MaxStretch;
                    }
                }
                else
                {
                    if (_squashValue < -MaxSquash)
                    {
                        _squashValue = -MaxSquash;
                    }
                }

                _squashValue -= (_squashValue * 10.0f) * Time.deltaTime;

                _dummy.SetSquash(_upVector3, _squashValue);
            }
        }
    }

    private Vector3 ConvertToLocalSpace(Vector3 v)
    {
        Vector3 temp;
        temp.x = Vector3.Dot(_moveDirRight.normalized, v.normalized);
        temp.y = Vector3.Dot(_upVector3.normalized, v.normalized);
        temp.z = Vector3.Dot(_moveDirForward.normalized, v.normalized);

        temp = temp.normalized * v.magnitude;

        return temp;
    }

    public void SetForwardDir(Vector3 forward)
    {
        _moveDirForward = forward;
        //_playerRigidBody.velocity = _moveDirForward.normalized * _playerRigidBody.velocity.magnitude;
    }

    public Vector3 GetForwardDir()
    {
        return _moveDirForward;
    }

    public void ApplyForce(Vector3 appliedForce)
    {
        _squashValue = -MaxSquash;
        _playerRigidBody.velocity = appliedForce;
        _jumping = true;
    }

    public void ApplyLocalForce(Vector3 appliedForce)
    {
        _squashValue = -MaxSquash;
        _playerRigidBody.velocity = ToWorldSpace(appliedForce);
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

    public Vector3 GetLocalVelocity()
    {
        return _velocity;
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

    public Vector3 ToWorldSpace(Vector3 v)
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

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

    public void SetMaterial(Material mat)
    {
        _dummy.SetMaterial(mat);
    }
}