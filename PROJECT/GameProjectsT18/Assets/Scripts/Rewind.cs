using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour {

    private ArrayList previousPosition = new ArrayList();
    private ArrayList previousRotation = new ArrayList();
    private int positionIdx = 0;
    private GameObject PlayerGameObject;
    private bool bRewinding = false;
    public int RewindDuration; // for later use once a timer is available

	void Start ()
    {
        PlayerGameObject = GameObject.FindWithTag("Player");
    }
	
	void Update ()
    {
        if(positionIdx > previousPosition.Count - 1 | positionIdx < 0)
        {
            positionIdx = previousPosition.Count;
        }

		if(!bRewinding)
        {
            Transform playerTransform = PlayerGameObject.GetComponent<Transform>();
            Debug.Log("Player Position:" + playerTransform.position
                + "Player Rotation:" + playerTransform.localRotation
                + "Position Array:" + previousPosition.Count 
                + "Position Idx" + positionIdx);
            // Save Pos and Rot
            previousPosition.Add(playerTransform.position);
            previousRotation.Add(playerTransform.localRotation);
            positionIdx++;
        }


        if(Input.GetKey("r"))
        {
            bRewinding = true;
            RewindTime();
        } else
        {
            bRewinding = false;
        }
    }
    void RewindTime()
    {
        positionIdx--;
        if(positionIdx > 0)
        {
            Transform playerTransform = PlayerGameObject.GetComponent<Transform>();
            playerTransform.position = (Vector3)previousPosition[positionIdx];
            playerTransform.localRotation = (Quaternion)previousRotation[positionIdx];
            previousPosition.RemoveAt(positionIdx);
            previousRotation.RemoveAt(positionIdx);
            Debug.Log("Player Position:" + playerTransform.position + "Position Array:" + previousPosition.Count + "Position Idx" + positionIdx);
        }
    }
}
