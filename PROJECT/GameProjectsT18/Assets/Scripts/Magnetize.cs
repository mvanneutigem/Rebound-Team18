using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Magnetize : MonoBehaviour
{
    public enum Type { Attract, Repel }

    [SerializeField]
    private float RepelForce = 1000.0f;

    [SerializeField]
    private float MinimumDistance = 1.0f;

    [SerializeField]
    private Type MagnetizeType = Type.Repel;

    private GameObject[] _magneticObject;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _magneticObject = GameObject.FindGameObjectsWithTag("Magnetic");
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_magneticObject != null)
        {
            for(int i = 0; i < _magneticObject.Length; ++i)
            {
                Vector3 difference;
                if (MagnetizeType == Type.Repel)
                    difference = transform.position - _magneticObject[i].transform.position;
                else
                    difference = _magneticObject[i].transform.position - transform.position;

                if (difference.magnitude <= MinimumDistance)
                {
                    _rigidbody.AddForce(difference * RepelForce * Time.deltaTime);
                }
            }
           
        }
    }
}
