using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Magnetize : MonoBehaviour
{
    [SerializeField]
    private float RepelForce = 1000.0f;

    [SerializeField]
    private float AttractForce = 1000.0f;

    [SerializeField]
    private float MinimumDistance = 1.0f;

    public bool UseNormal = false;
    private GameObject[] _magneticObject;
    private GameObject[] _attractmagneticObject;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _magneticObject = GameObject.FindGameObjectsWithTag("Magnetic");
        _attractmagneticObject = GameObject.FindGameObjectsWithTag("Magnetic_Attract");
        _rigidbody = GetComponent<Rigidbody>();
    }

    Vector3 Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z *b.z);
    }

    void Update()
    {
        if (_magneticObject != null)
        {
            for(int i = 0; i < _magneticObject.Length; ++i)
            {
                Vector3 difference;
                difference = transform.position - _magneticObject[i].transform.position;


                if (difference.magnitude <=  MinimumDistance)
                {
                    float scale = 1 - difference.magnitude / MinimumDistance;
                    difference.Scale(new Vector3( scale,scale,scale));
                    if (UseNormal)
                    {
                        _rigidbody.AddForce(Multiply(difference, _magneticObject[i].transform.forward) * RepelForce * 10 * Time.deltaTime);
                    }
                    else
                    {
                        _rigidbody.AddForce(difference * RepelForce * 10 * Time.deltaTime);
                    }
                }
            }
           
        }
        if (_attractmagneticObject != null)
        {
            for (int i = 0; i < _attractmagneticObject.Length; ++i)
            {
                Vector3 difference;
                difference = _attractmagneticObject[i].transform.position - transform.position;

                if (difference.magnitude <= MinimumDistance)
                {
                    float scale = 1 - difference.magnitude / MinimumDistance;
                    difference.Scale(new Vector3(scale, scale, scale));
                    _rigidbody.AddForce(difference * AttractForce * 10 * Time.deltaTime);
                }
            }
        }
    }
}
