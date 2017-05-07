using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFade : MonoBehaviour
{

    public GameObject Player;
    private List<GameObject> hitObjects = new List<GameObject>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var direction = Player.transform.position - transform.position;

        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        int distance = 15;
        if (Physics.Raycast(ray, out hit, distance))
        {

            if (hit.collider != Player)
            {
                GameObject removeGameObject = new GameObject();
                bool remove = false;
                foreach (GameObject o in hitObjects)
                {
                    if (hit.collider != o)
                    {
                        Color col = o.GetComponent<Renderer>().material.color;
                        col.a = 1.0f;
                        o.GetComponent<Renderer>().material.color = col;
                        remove = true;
                        removeGameObject = o;
                    }
                }
                if (remove)
                    hitObjects.Remove(removeGameObject);

                Color c = hit.collider.GetComponent<Renderer>().material.color;
                c.a = 0.2f;
                hit.collider.GetComponent<Renderer>().material.color = c;
                hitObjects.Add(hit.collider.gameObject);
            }
        }
    }
}
