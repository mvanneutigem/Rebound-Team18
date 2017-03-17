using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    private Animator myAnimator;
    private PlayerController controller;
    private CharacterController _characterController;
    private float VSpeed = 0;
	void Start ()
	{
	    myAnimator = GetComponent<Animator>();
	    controller = GetComponent<PlayerController>();
        _characterController = GetComponent<CharacterController>();
    }
	
	void Update ()
	{
	    var movement = controller.GetVelocity();

	    //if (_characterController.isGrounded)
	    //{
	    //    if ((movement.y) < 0.5f)
	    //    {
	    //        myAnimator.SetBool("Jumping", false);
	    //        if (movement.z > 0)
	    //        {
     //               myAnimator.SetBool("Running", true);
	    //        }
	    //        else
	    //        {
	    //            myAnimator.SetBool("Running", false);
	    //        }
	    //    }
	    //    else
	    //    {
	    //        myAnimator.SetBool("Jumping", true);
	    //    }
	    //}
	    //else
	    //{
	    //    if (movement.y < -1.0f)
	    //    {
	    //        myAnimator.SetBool("Falling", false);
	    //        myAnimator.SetBool("Landing", true);
	    //    }
     //       else if (movement.y > 1.0f)
	    //    {
     //           myAnimator.SetBool("Jumping", true);
     //           myAnimator.SetBool("Landing", false);
     //       }
     //       else
     //       {
     //           myAnimator.SetBool("Falling", true);
     //           myAnimator.SetBool("Jumping", true);
     //       }
               
	    //}
    }
}
