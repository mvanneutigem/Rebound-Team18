using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPortal : MonoBehaviour
{
    public Vector3 GravityDirectionVector = new Vector3(0, -1, 0);
    public Vector3 ChangeForwardVector = new Vector3(0, 0, 1);
    private PhysicsPlayerController _playerController;
    private Transform _playerTransform;
    private Transform _transSelf;
    public float RotateDistance = 20.0f;
    public float ChangeDirectionSpeed = 20.0f;
    private bool _entered = false;
    private Vector3 _playerStartUpVector;
    private Vector3 _playerStartForwardVector;
    private Vector3 _playerStartRightVector;
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
            _playerController = playerGameObject.GetComponent<PhysicsPlayerController>();
        }
    }
    void Update()
    {
        //DrawLine(_playerTransform.position, _playerTransform.position + (_playerController.GetUpVector() * 4), Color.green, .02f);
        //DrawLine(_playerTransform.position, _playerTransform.position + (_playerController.GetRightVector() * 4), Color.red, .02f);
        //DrawLine(_playerTransform.position, _playerTransform.position + (_playerController.GetForwardDir() * 4), Color.blue, .02f);

        if (_entered)
        {

            // Creating a copy of it's transform to invisible change with the forward vector to get a static right and up vector to make calculations off.
            // *********
            GameObject pseudoObject = new GameObject();
            pseudoObject.transform.right = _transSelf.right;
            pseudoObject.transform.forward = _transSelf.forward;
            pseudoObject.transform.up = _transSelf.up;
            pseudoObject.transform.forward = _playerController.GetForwardDir();
            // *********

            _timer += Time.deltaTime;
            Debug.Log("Time: " + _timer);
            if (_playerTransform.forward == ChangeForwardVector)
            {
                Debug.Log("TRUE");
            }

            Vector3 direction = _playerTransform.position - _transSelf.position;
            float distance = Vector3.Dot(direction, _playerController.GetForwardDir());

            float range = (distance / RotateDistance);
            Debug.Log("distance" + distance + "Range" + range);


            // Local temporary variables
            Vector3 up = Vector3.zero;
            Vector3 forward = _playerController.GetForwardDir();

            direction = Vector3.Normalize(direction);
            if (range < 1.05f && range > Mathf.Epsilon)
            {
                // Angle gets continiously updated 
                _angle = Vector3.Angle(_playerController.GetUpVector(), -GravityDirectionVector);

                // Incase the rotation is CCW the right vector becomes the left vector
                if(GravityDirectionVector.x > 0)
                {
                    pseudoObject.transform.right = -pseudoObject.transform.right;
                }

                DrawLine(_playerTransform.position, _playerTransform.position + (pseudoObject.transform.right * 4), Color.yellow, .02f);


                if (range > 0 && _playerController.GetUpVector() != -GravityDirectionVector)
                {
                    if (range < 0.5f)
                    {
                        up = Vector3.Lerp(_playerStartUpVector, pseudoObject.transform.right, range * 2.0f);
                    }
                    else if (_angle > 0)
                    {

                        up = Vector3.Lerp(pseudoObject.transform.right, -GravityDirectionVector, (range - 0.5f) * 2.0f);
                    }
                    else
                    {
                        return;
                    }
                }

            }
            // Both the up Gravity direction and the Forward direction CAN change the upvector. so I calculate them twice, locally, and then lerp them 50/50
            // 1st calculations is with the up
            Vector3 upSecond = Vector3.zero;

            if (_playerController.GetForwardDir() != ChangeForwardVector)
            {
                forward = Vector3.RotateTowards(forward, ChangeForwardVector, Mathf.PI / 360 * (ChangeDirectionSpeed), Mathf.PI);
                // second up calculations here
                upSecond = Vector3.Cross(_playerController.GetForwardDir(), _playerController.GetRightVector());
                forward.Normalize();
                _playerController.SetForwardDir(forward);
            }

            // If both up vectors haven't been calculated thiss pass (equaling zero) then there is no need to update the up vector
            if (up != Vector3.zero && upSecond != Vector3.zero)
            {
                // 50/50 lerp here
                up = Vector3.Lerp(up, upSecond, 0.5f );
                up.Normalize();
                _playerController.SetUpVector(up);
            }else if(up == Vector3.zero && upSecond != Vector3.zero){
                upSecond.Normalize();
                _playerController.SetUpVector(upSecond);
            }else if(upSecond == Vector3.zero && up != Vector3.zero){
                up.Normalize();
                _playerController.SetUpVector(up);
            }

            if (_playerController.GetUpVector() == -GravityDirectionVector && _playerController.GetForwardDir() == ChangeForwardVector)
            {
                _entered = false;
            }
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
                // Upon entering, the angle for rotation between the up and gravity is determined
                // Alongside with all the vectors upon entering.
                _playerStartUpVector = _playerController.GetUpVector();
                _playerStartForwardVector = _playerController.GetForwardDir();
                _angle = Vector3.Angle(_playerStartUpVector, -GravityDirectionVector);
                _entered = true;

                // If angle is less then 178 then the rotation should go either clockwise or counterclockwise, depending on the gravity vector
                // So the Vector get's changed to either the Actual right vector or the gravitydirection. depending on the angle.
                // this means that 180 degree turns will always go CW
                if(_angle > 178)
                {
                    _playerStartRightVector = _playerController.GetRightVector();
                }
                else
                {
                    _playerStartRightVector = _playerController.GetUpVector() - GravityDirectionVector;
                }
            }
        }
    }
}
