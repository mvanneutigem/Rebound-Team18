using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPortal : MonoBehaviour {
    public Vector3 rotation = new Vector3(0, 0, -90);
    public GameObject LevelGameObject;
    public float Speed = 1.0f;
    private bool Entered = false;

    void Update()
    {
        if (Entered)
        {
            Quaternion targetRotation = Quaternion.Euler(rotation);
            LevelGameObject.transform.rotation = Quaternion.Lerp(LevelGameObject.transform.rotation, targetRotation, Time.deltaTime * Speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !Entered)
        {
            Entered = true;
        }
    }
}
