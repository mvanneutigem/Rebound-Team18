using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    //FIELDS
    private PlayerController _playerController;
    private float _angle = 0;
    public float Speed = 1;
    public float Amplitude=0.5f;
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
        }
    }

	void Update () {
        //_angle += Speed*Time.deltaTime;
        //Vector3 pos = _originalPos;
        //pos.y += Mathf.Sin(_angle)*Amplitude;
        //_transSelf.Rotate(0f,0f,1.0f);
        //_transSelf.position = pos;
        _angle += Speed * Time.deltaTime;
        _transSelf.Rotate(0f, 1.0f, 0f);
        _transSelf.position = _originalPos;
        _transSelf.Translate(0f, Mathf.Sin(_angle)*Amplitude,0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);  
        }
    }
}
