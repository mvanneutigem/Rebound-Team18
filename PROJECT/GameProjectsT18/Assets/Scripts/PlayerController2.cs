using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerController2 : MonoBehaviour
{
    //PROPERTIES
    public float Speed { get { return _speed; } set { _speed = value; } }

    //FIELDS
    private Vector3 _moveVector;
    private Vector3 _airVector;
    private CharacterController _characterController;
    public GameObject mainCamera;
    private float _rotateSpeed = 0.03f;
    [SerializeField]
    private float _speed = 5.0f;

    //METHODS
    void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
    }
    void Update()
    {
        // input
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        var camforward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        if (_characterController.isGrounded)
        {
            _moveVector = (vInput * camforward + hInput * mainCamera.transform.right);
            //set the speed
            _moveVector = Speed * _moveVector.normalized;
            _airVector = Vector3.zero;
        }
        else if (_airVector == Vector3.zero)
        {
            _airVector = _moveVector;
        }
        else
        {
            _airVector += Physics.gravity * Time.deltaTime;
            _moveVector = (vInput * camforward + hInput * mainCamera.transform.right) + _airVector;
            _moveVector = Speed * _moveVector.normalized;
        }

        if (Input.GetAxis("Jump") > 0)
        {
            if (_characterController.isGrounded)
            {
                _moveVector += Vector3.up * _speed;
            }
            _moveVector.x = hInput * Speed / 2;
            _moveVector.z = vInput * Speed / 2;
        }

        //rotate character in the direction it's moving in
        if (_moveVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.time * _rotateSpeed);
        }

        //Gravity
        _moveVector += Physics.gravity * Time.deltaTime;

        //Move
        _characterController.Move(_moveVector * Time.deltaTime);
    }

    public void ApplyForce(Vector3 force)
    {
        if (_characterController.isGrounded)
        {
            _moveVector += force;
        }

        Debug.Log(_moveVector);
        _characterController.Move(_moveVector * Time.deltaTime);
    }
}
