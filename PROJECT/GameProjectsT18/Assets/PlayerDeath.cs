using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerDeath : MonoBehaviour {
    private List<Transform> _trampolineTransforms;
    private Queue<Transform> _nearestTrampolines;
    private PlayerController _playerContr;
    int _currentLastIndex;
	// Use this for initialization
	void Start () {
        _playerContr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ReflectedTrampoline[] trampolines = GameObject.FindObjectsOfType<ReflectedTrampoline>();
        _trampolineTransforms = new List<Transform>();
        _trampolineTransforms.Capacity = trampolines.Length;

        for (int i = 0; i < trampolines.Length; i++)
        {
            _trampolineTransforms.Add(trampolines[i].transform);
        }

        _trampolineTransforms = _trampolineTransforms.OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).ToList();
        _nearestTrampolines = new Queue<Transform>();
        for (int i = 0; i < _trampolineTransforms.Count; i++)
        {
            if (Vector3.Distance(this.transform.position, _trampolineTransforms[i].position) < 50.0f)
            {
                _nearestTrampolines.Enqueue(_trampolineTransforms[i]);
                _currentLastIndex = i;
            }
            else
            {
                continue;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        for (int i = _currentLastIndex; i < _trampolineTransforms.Count; i++)
        {
            if (Vector3.Distance(this.transform.position, _trampolineTransforms[i].position) < 50.0f)
            {
                _nearestTrampolines.Enqueue(_trampolineTransforms[i]);
                _currentLastIndex = i + 1;
            }
            else
            {
                continue;
            }
        }

        for (int i = 0; i < _nearestTrampolines.Count; i++)
        {
            Vector3 dir = _nearestTrampolines.Peek().position - this.transform.position;
            float dot = Vector3.Dot(_playerContr.GetForwardDir(), dir.normalized);

            if (dot < 0)
            {
                _nearestTrampolines.Dequeue();
            }
            else
            {
                continue;
            }
        }

        {
            bool canLand = false;

            Vector3 dir = _nearestTrampolines.Peek().position - this.transform.position;

            float dot = Vector3.Dot(_playerContr.GetUpVector(), dir.normalized);

            if (dot > 0)
            {
                Vector3 test1 = this.transform.position;
                Transform test2 = _nearestTrampolines.Peek();

                float dist = Vector3.Distance(test1, test2.position);
                if (dist < 5.0f)
                {
                    canLand = true;
                }
            }
            else
            {
                canLand = true;
            }


            if (!canLand) Debug.Log("DIE DIE DIE");
        }
    }
}
