using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rewind : MonoBehaviour {

    // Array Lists
    public float RefillMultiplier = 0.5f;

    private ArrayList _previousPositions = new ArrayList();
    private ArrayList _previousRotations = new ArrayList();
    private ArrayList _previousVelocities = new ArrayList();
    private ArrayList _previousForward = new ArrayList();
    private ArrayList _previousUp = new ArrayList();
    private ArrayList _previousMat = new ArrayList();
    private ArrayList _previousNewUp = new ArrayList();
    private ArrayList _previousNewForward = new ArrayList();
    private ArrayList _previousRotationSpeed = new ArrayList();
    private ArrayList _previousPseudoRight = new ArrayList();

    private int _arrayIdx = 0;

    private GameObject _glow;
    private GameObject _playerGameObject;
    private Transform _rewindBar;
    private float _barLength;
    private PhysicsPlayerController _playerController;
    private Transform _playerTransform;
    private float _rewindAmount = 0;
    public float MaxRewindTime = 4.0f;
    private bool _rewinding = false;
    private PhysicsPlayerController.Mat _material;
    private GameObject _switcher;
    private string rewindString;

    private Vector3 _newUp = new Vector3(0, 1, 0);
    private Vector3 _newForward = new Vector3(0, 0, 1);
    private float _rotationSpeed;
    private Vector3 _pseudoRight;
    private GravityNew _gravityScript;
    private GameObject _dummyPortal;

    void Start ()
    {
        GameObject origPortal = GameObject.Find("GravityPortal");
        _dummyPortal = Instantiate(origPortal);
        _dummyPortal.transform.position = new Vector3(float.MaxValue / 2.0f, float.MaxValue / 2.0f, float.MaxValue / 2.0f);
        Renderer[] r = _dummyPortal.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < r.Length; i++)
        {
            r[i].enabled = false;
        }

        _dummyPortal.name = "dummyPortal";
        _gravityScript = _dummyPortal.GetComponentInChildren<GravityNew>();

        _glow = GameObject.Find("uiglow");
        _glow.SetActive(false);
        _playerGameObject = GameObject.FindWithTag("Player");
        _playerController = _playerGameObject.GetComponent<PhysicsPlayerController>();
        _playerTransform = _playerGameObject.GetComponent<Transform>();
        _rewindBar = GameObject.Find("RewindBar").transform;
        _barLength = _rewindBar.localScale.x;
        _switcher = GameObject.Find("MaterialChanger");
        rewindString = PlayerPrefs.GetString("Rewind");


        Vector3 scale = _rewindBar.localScale;
        scale.x = 0;
        _rewindBar.localScale = scale;

        this.enabled = false;
    }
	
	void Update ()
    {
        if (Time.timeScale > float.Epsilon)
        {
            if (_arrayIdx > _previousPositions.Count - 1 | _arrayIdx < 0)
            {
                _arrayIdx = _previousPositions.Count;
            }

            if (!_rewinding)
            {
                if (_rewindAmount < MaxRewindTime)
                {
                    _rewindAmount += RefillMultiplier * Time.deltaTime;
                }
                else
                {
                    _rewindAmount = MaxRewindTime;
                }

                _previousMat.Add(_playerController.materialstate);
                _previousPositions.Add(_playerTransform.position);
                _previousRotations.Add(_playerTransform.localRotation);
                _previousVelocities.Add(_playerController.GetVelocity());
                _previousForward.Add(_playerController.GetForwardDir());
                _previousUp.Add(_playerController.GetUpVector());
                //Debug.Log("Forward in Rewind: " + _newForward);
                _previousNewForward.Add(_newForward);
                //Debug.Log("Up in Rewind: " + _newUp);
                _previousNewUp.Add(_newUp);
                //Debug.Log("changeSpeed in Rewind: " + _rotationSpeed);
                _previousRotationSpeed.Add(_rotationSpeed);
                //Debug.Log("Pseudo right in Rewind: " + _pseudoRight);
                _previousPseudoRight.Add(_pseudoRight);
                
                
                _arrayIdx++;
            }
            else
            {
                _rewindAmount -= Time.deltaTime; ;
                if (_rewindAmount > 0)
                {
                    RewindTime();
                }
                else
                {
                    _playerController.SetLockMovement(false);
                    _rewindAmount = 0;
                }
            }
            Vector3 scale = _rewindBar.localScale;
            scale.x = (_rewindAmount / MaxRewindTime) * _barLength;
            _rewindBar.localScale = scale;


            if ((Input.GetAxisRaw("Rewind") > 0  || Input.GetKeyDown(rewindString)) && !_rewinding)
            {
                _glow.SetActive(true);
                _rewinding = true;
                _playerController.SetLockMovement(true);
            }

            if ((Input.GetAxisRaw("Rewind") == 0 && !Input.GetKey(rewindString)) && _rewinding)
            {
                _glow.SetActive(false);
                _rewinding = false;
                _playerController.SetLockMovement(false);
                _playerController.SetVelocity((Vector3)_previousVelocities[_arrayIdx - 1]);
                _gravityScript.SetEntered(true);
            }
        }
        
    }
    void RewindTime()
    {
        _arrayIdx--;
        if(_arrayIdx > 0)
        {
            if (_switcher)
            {
                _material = (PhysicsPlayerController.Mat)_previousMat[_arrayIdx - 1];
                if (_playerController.materialstate != _material)
                {
                    _playerController.materialstate = _material;
                    _switcher.GetComponent<MaterialSwitcher>().ChangeMaterial(_material, _playerGameObject);
                }
                _previousMat.RemoveAt(_arrayIdx);
            }

            _playerTransform.position = (Vector3)_previousPositions[_arrayIdx-1];
            _playerTransform.localRotation = (Quaternion)_previousRotations[_arrayIdx-1];
            _playerController.SetForwardDir((Vector3)_previousForward[_arrayIdx - 1]);
            _playerController.SetUpVector((Vector3)_previousUp[_arrayIdx - 1]);
            _gravityScript.SetNewForwardVector((Vector3)_previousNewForward[_arrayIdx - 1]);
            _gravityScript.SetNewUpVector((Vector3)_previousNewUp[_arrayIdx - 1]);
            //Debug.Log("setting changeSpeed while Rewinding: " + (float)_previousRotationSpeed[_arrayIdx - 1]);
            _gravityScript.SetRotationSpeed((float)_previousRotationSpeed[_arrayIdx - 1]);
            _gravityScript.SetPseudoRight((Vector3)_previousPseudoRight[_arrayIdx - 1]);

            _previousPositions.RemoveAt(_arrayIdx);
            _previousRotations.RemoveAt(_arrayIdx);
            _previousVelocities.RemoveAt(_arrayIdx);
            _previousForward.RemoveAt(_arrayIdx);
            _previousUp.RemoveAt(_arrayIdx);
            _previousNewForward.RemoveAt(_arrayIdx);
            _previousNewUp.RemoveAt(_arrayIdx);
            _previousRotationSpeed.RemoveAt(_arrayIdx);
            _previousPseudoRight.RemoveAt(_arrayIdx);
        }
    }
    public bool IsRewinding()
    {
        return _rewinding;
    }

    public void SetNewForward(Vector3 vec)
    {
        _gravityScript.SetNewForwardVector(vec);
        _newForward = vec;
    }

    public void SetNewUp(Vector3 vec)
    {
        _gravityScript.SetNewUpVector(vec);
        _newUp = vec;
    }

    public void SetRotationSpeed(float value)
    {
        _rotationSpeed = value;
        _gravityScript.SetRotationSpeed(value);
    }

    public void SetPseudoRight(Vector3 right)
    {
        _gravityScript.SetPseudoRight(right);
        _pseudoRight = right;
    }

    public void EnableDummy(bool yup)
    {
        _dummyPortal.SetActive(yup);
    }
}
