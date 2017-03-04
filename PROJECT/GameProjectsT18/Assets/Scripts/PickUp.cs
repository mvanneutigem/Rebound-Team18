using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    //FIELDS
    private PlayerController _playerController;
    private Transform _playerTransform;
    private float _angle = 0;
    public float Speed = 1;
    public float Amplitude=0.5f;
    public float PickupRange = 3.0f;
    public float MoveTime = 0.5f;
    private float _timer = 0;
    private Vector3 _originalPos;
    private Transform _transSelf;

    //METHODs
	
    void Start()
    {
        GameObject gameControllerObj = GameObject.FindWithTag("Player");
        _transSelf = this.transform;
        _originalPos = _transSelf.position;
        if (gameControllerObj != null)
        {
            _playerController = gameControllerObj.GetComponent<PlayerController>();
            _playerTransform = gameControllerObj.transform;
        }
    }

	void Update () {
        //_angle += Speed*Time.deltaTime;
        //Vector3 pos = _originalPos;
        //pos.y += Mathf.Sin(_angle)*Amplitude;
        //_transSelf.Rotate(0f,0f,1.0f);
        //_transSelf.position = pos;


        if (Mathf.Abs((_playerTransform.position - _transSelf.position).sqrMagnitude) < PickupRange * PickupRange)
        {
            _timer += Time.deltaTime;
            _transSelf.position = Vector3.Lerp(_originalPos, _playerTransform.position, _timer / MoveTime);
        }
        else
        {
            _angle += Speed * Time.deltaTime;
            _transSelf.Rotate(0f, 1.0f, 0f);
            _transSelf.position = _originalPos;
            _transSelf.Translate(0f, Mathf.Sin(_angle) * Amplitude, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);  
        }
    }
}
