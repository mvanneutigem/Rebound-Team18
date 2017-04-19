using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float FOV = 60.0f;

    private Transform _camTransform;

    public Camera _camera;
    [SerializeField]
    private float _mouseSpeedH = 10.0f;
    [SerializeField]
    private float _mouseSpeedV = -6f;
    [SerializeField]
    private float _controllerSpeedH = 1f;
    [SerializeField]
    private float _controllerSpeedV = 1f;
    [SerializeField]
    private PlayerController _player;
    private Transform _playerTransform;
    [SerializeField]
    private float _camBoomLength = 10f;
    [SerializeField]
    private bool _AllowRotation = false;

    private Transform _transSelf;

    private GameObject sphereGameObject;

    void Awake()
    {
        _transSelf = transform;
        _camTransform = _camera.transform;
        _camTransform.SetParent(this.transform);
        _camTransform.forward = _transSelf.position - _camTransform.position;

        _camera = _camTransform.GetComponent<Camera>();
        _camera.fieldOfView = FOV;

        _camTransform.localPosition = new Vector3(0, 2, -_camBoomLength);

        _playerTransform = _player.transform;
        //sphereGameObject = GameObject.FindWithTag("Player");
    }
    
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        //old code:
        if (_AllowRotation)
        {
            float moveX = Input.GetAxis("Controller X") * _controllerSpeedH + Input.GetAxis("Mouse X") * _mouseSpeedH;
            float moveY = Input.GetAxis("Controller Y") * _controllerSpeedV + Input.GetAxis("Mouse Y") * _mouseSpeedV;

            Vector3 camAngle = _transSelf.localEulerAngles;

            if (Mathf.Abs(moveX) > Mathf.Epsilon)
            {
                camAngle = new Vector3(camAngle.x, camAngle.y + moveX, camAngle.z);
            }

            if (Mathf.Abs(moveY) > Mathf.Epsilon)
            {
                camAngle = new Vector3(camAngle.x + moveY, camAngle.y, camAngle.z);
            }

            if (camAngle.x < 275 && camAngle.x > 180) camAngle.x = 275;
            if (camAngle.x > 85 && camAngle.x < 180) camAngle.x = 85;
            //else if (camAngle.x > _verMax) camAngle.x = _verMax;

            _transSelf.localEulerAngles = camAngle;
            //_camTransform.forward = _transSelf.position - _camTransform.position;
        }
        _transSelf.position = _playerTransform.position;

        // followcam
        //Vector3 sphereVelocity = sphereGameObject.GetComponent<Rigidbody>().velocity;
        //sphereVelocity.Normalize();

        //Vector3 spherePos = sphereGameObject.transform.position;
        //Vector3 camPos = transform.position;
        //Vector3 camToSphere = spherePos - camPos;
        //camToSphere.Normalize();

        //Vector3 meanDir = camToSphere + sphereVelocity;
        //meanDir.Normalize();
        //float minDist = 5.0f;

        //Vector3 targetPos = spherePos - meanDir * minDist;
        //Vector3 targetVector = (targetPos - camPos) / 30.0f;
        //Vector3 newPos = camPos + targetVector;

        //transform.position = newPos;

        //transform.LookAt(spherePos);

        //transform.right = sphereGameObject.transform.right;
    }

    public void SetFOV(float fov)
    {
        _camera.fieldOfView = fov;
    }

    public float GetFOV()
    {
        return _camera.fieldOfView;
    }
}
