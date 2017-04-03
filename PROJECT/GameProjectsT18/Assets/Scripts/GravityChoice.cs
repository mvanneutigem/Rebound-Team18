using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChoice : MonoBehaviour
{

    private GravityPortal _gravityPortal;
	void Start ()
	{
	    _gravityPortal = GetComponentInParent<GravityPortal>();

	}
	
	void OnTriggerStay (Collider other)
	{
	    if (other.tag == "Player")
	    {
	        float input = Input.GetAxis("Direction");
	        _gravityPortal.SetGravityDirection(new Vector3(input, 0, 0));
	    }
	}
}
