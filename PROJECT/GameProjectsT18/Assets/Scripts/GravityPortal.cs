using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPortal : MonoBehaviour {
    public Vector3 rotationAxis = new Vector3(0, 0, -1);
    public float rotation = 90;
    public GameObject LevelGameObject;
    public float RotateTime = 0.5f;
    private float _timer = 0; 
    private bool Entered = false;
    private Quaternion _startRotation;

    void Update()
    {
        if (Entered)
        {
            _timer += Time.deltaTime;
            Quaternion targetRotation = Quaternion.AngleAxis(rotation, rotationAxis);
            LevelGameObject.transform.rotation = Quaternion.Lerp(_startRotation, targetRotation, _timer / RotateTime);

            if (_timer >= RotateTime)
            {
                // disable self
                this.GetComponent<GravityPortal>().enabled = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !Entered)
        {
            Entered = true;
            _startRotation = LevelGameObject.transform.rotation;
        }
    }
}
