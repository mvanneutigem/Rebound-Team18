using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPortal : MonoBehaviour
{
    public Vector3 GravityDirectionVector = new Vector3(0, -1, 0);
    public Vector3 ChangeForwardVector = new Vector3(0, 0, 1);
    private PlayerController _playerController;
    private Transform _playerTransform;
    private Transform _transSelf;
    public float RotateDistance = 20.0f;
    public float ChangeDirectionSpeed = 20.0f;
    private bool _entered = false;
    private Vector3 _playerStartUpVector;
    private Vector3 _playerStartForwardVector;
    private Vector3 _rightVector3;
    private float _step = 0;
    private float _timer = 0;
    private float _angle = 0;
    private float _switchAngle = 0;
    void Start()
    {
        _transSelf = this.transform;
        var playerGameObject = GameObject.FindWithTag("Player");
        if (playerGameObject != null)
        {
            _playerTransform = playerGameObject.transform;
            _playerStartUpVector = _playerTransform.up;
            _playerStartForwardVector = _playerTransform.forward;
            _playerController = playerGameObject.GetComponent<PlayerController>();
        }
    }
    void Update()
    {
        DrawLine(_playerTransform.position, _playerTransform.position + (_playerController.GetForwardDir() * 10), Color.red, .02f);
        if (_entered)
        {
            _timer += Time.deltaTime;
            Debug.Log("Time: " + _timer);
            if(_playerTransform.forward == ChangeForwardVector)
            {
                Debug.Log("TRUE");
            }

            Vector3 direction = _playerTransform.position - _transSelf.position;
            float distance = Vector3.Dot(direction, _playerController.GetForwardDir());

            float range = (distance / RotateDistance);
            //Debug.Log("distance" + distance + "Range" + range);


            if (range < 1.05f && range > 0)
            {
                Vector3 up = _playerController.GetUpVector();
                Vector3 forward = _playerController.GetForwardDir();
                _angle = Vector3.Angle(_playerController.GetUpVector(), -GravityDirectionVector);
                //Debug.Log("Angle between " + -GravityDirectionVector + " and " +_playerTransform.up + " = " + _angle);


                if (range > 0)
                {
                    forward = Vector3.RotateTowards(forward, ChangeForwardVector, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
                    //forward = Vector3.Lerp(_playerStartForwardVector, ChangeForwardVector, range * 2.0f);
                    if (range < 0.5f)
                    {
                        up = Vector3.Lerp(_playerStartUpVector, _rightVector3, range * 2.0f);
                    }
                    else if (_angle > 0)
                    {

                        up = Vector3.Lerp(_rightVector3, -GravityDirectionVector, (range - 0.5f) * 2.0f);
                    }
                    else
                    {
                        return;
                    }
                }
                up.Normalize();
                _playerController.SetUpVector(up);
                forward.Normalize();
                _playerController.SetForwardDir(forward);
            }
            //Debug.Log("Up Vector " + _playerController.GetUpVector());

        }
    }

    public void SetGravityDirection(Vector3 direction)
    {
        GravityDirectionVector = direction;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!_entered && !_playerController.GetLockMovement())
            {
                _playerStartUpVector = _playerController.GetUpVector();
                _angle = Vector3.Angle(_playerStartUpVector, -GravityDirectionVector);
                if (_angle > 178)
                {
                    _rightVector3 = _playerController.GetRightVector();
                }
                else
                {
                    _rightVector3 = _playerStartUpVector - GravityDirectionVector;
                    _rightVector3.Normalize();
                }

                _switchAngle = _angle / 2.0f;

                _entered = true;
            }
        }
    }
}
