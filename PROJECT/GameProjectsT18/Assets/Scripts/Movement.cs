using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //FIELDS
    public Transform TransSelf;
    public Transform[] WayPoints;
    public float Speed = 2.0f;
    private int _currentWaypoint = 0;

    //METHODS
    void Update()
    {
        if (_currentWaypoint < WayPoints.Length)
        {
            TransSelf.position = Vector3.MoveTowards(TransSelf.position, WayPoints[_currentWaypoint].position, Speed * Time.deltaTime);
            if (TransSelf.position == WayPoints[_currentWaypoint].position)
            {
                GoToNextWaypoint();
            }
        }

    }

    private void GoToNextWaypoint()
    {
        ++_currentWaypoint;
        if (_currentWaypoint >= WayPoints.Length)
        {
            _currentWaypoint = 0;
        }
    }
}
