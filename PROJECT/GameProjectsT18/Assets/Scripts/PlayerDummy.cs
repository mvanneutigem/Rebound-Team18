using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour
{
    private Transform _transPlayer;
    private Transform _transChild;
    private Transform _transSelf;
    private Renderer _childRenderer;
    // Use this for initialization
    void Start ()
	{
	    _transPlayer = GameObject.FindGameObjectWithTag("Player").transform;
	    _transSelf = this.transform;
        _transSelf.SetParent(null);
        _transSelf.localPosition = new Vector3(0, 0, 0);
        _transSelf.localEulerAngles = new Vector3(0, 0, 0);
        _transChild = GameObject.Find("PlayerMesh").transform;
	    _childRenderer = GetComponentInChildren<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _transSelf.position = _transPlayer.position;
	    _transChild.localRotation = _transPlayer.rotation;
	}

    public void SetSquash(Vector3 direction, float amount)
    {
        Vector3 dir = direction;
        dir.x = Mathf.Abs(dir.x);
        dir.y = Mathf.Abs(dir.y);
        dir.z = Mathf.Abs(dir.z);
        _transSelf.localScale = new Vector3(1, 1, 1) + amount * dir;
        _transChild.localPosition = new Vector3(0, 0, 0) + amount * direction / 2.0f;
    }

    public void SetMaterial(Material mat)
    {
        _childRenderer.material = mat;
    }
}
