using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    //PROPERTIES
    public float Speed { get { return _speed; } set { _speed = value; } }

    //FIELDS
    private Vector3 _moveVector;
    private CharacterController _characterController;
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

        if (_characterController.isGrounded)
        {
            _moveVector = new Vector3(hInput * Speed, 0, vInput * Speed);
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

        //Gravity
        _moveVector += Physics.gravity * Time.deltaTime;

        //Move
        _characterController.Move(_moveVector * Time.deltaTime);
    }
}
