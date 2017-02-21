using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    //FIELDS
    private PlayerController _playerController;

    //METHODs
	
    void Start()
    {
        GameObject gameControllerObj = GameObject.FindWithTag("Player");
        if (gameControllerObj != null)
        {
            _playerController = gameControllerObj.GetComponent<PlayerController>();
        }
    }

	void Update () {
	    this.transform.Rotate(0f,0f,1.0f);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);  
        }
    }
}
