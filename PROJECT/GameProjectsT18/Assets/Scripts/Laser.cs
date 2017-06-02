using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private PhysicsPlayerController _playerController;
    public GameObject Cylinder;

    void Start ()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PhysicsPlayerController>();
    }

	void Update ()
    {
        if (_playerController.materialstate == PhysicsPlayerController.Mat.GLASS)
        {
            Cylinder.SetActive(false);
        }
        else
        {
            Cylinder.SetActive(true);
        }
	}
}
