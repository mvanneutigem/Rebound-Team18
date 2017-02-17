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
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.5f);
        }

        Vector3 temp = vInput * Speed * _moveDirForward + hInput * Speed * _moveDirRight;
        _moveVector.x = temp.x;
        _moveVector.z = temp.z;

        if (Input.GetAxis("Jump") > 0 && !_jumping)
        {
            _moveVector = Vector3.up * JumpSpeed;
            _jumping = true;
        }
        else
        {
            if (_characterController.isGrounded) _jumping = false;
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
}

