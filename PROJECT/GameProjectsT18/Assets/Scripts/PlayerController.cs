using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    //PROPERTIES
    public float Speed { get { return _speed; } set { _speed = value; } }

    //FIELDS
    private Vector3 _moveVector;
    private Vector3 _moveDirForward;
    private Vector3 _moveDirRight;
    private CharacterController _characterController;
    [SerializeField]
    private float _speed = 5.0f;
    public float JumpSpeed = 2.0f;
    private bool _jumping = false;
    private float _rotateSpeed = 0.03f;
    private Vector3 _trampolineInfluence = Vector3.zero;
    public float TrampolineUpPower = 5.0f;

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
        if (hInput + vInput != 0)
        {
            _moveDirForward = _camForward;
            _moveDirRight = new Vector3(_moveDirForward.z, _moveDirForward.y, -_moveDirForward.x);

            Quaternion targetRotation = Quaternion.LookRotation(_camForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.time * _rotateSpeed);
        }

        Vector3 tempMove = vInput * _moveDirForward * Speed + hInput * _moveDirRight * Speed;
        //ad trampolineforce if it's sideways
        Vector3 temp = tempMove + _trampolineInfluence * Speed ;

        _moveVector.x = temp.x;
        _moveVector.z = temp.z;
        
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
        _trampolineInfluence.y = 0;
        _trampolineInfluence.Normalize();
        _characterController.Move(_moveVector * Time.deltaTime);
    }
}

