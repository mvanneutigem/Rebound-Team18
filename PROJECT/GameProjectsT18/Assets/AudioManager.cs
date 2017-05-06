using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource[] _sources;
	// Use this for initialization
	void Start ()
	{
        _sources = GetComponents<AudioSource>();
	}

    public void PlaySound(string name)
    {
        for (int i = 0; i < _sources.Length; i++)
        {
            if (name == _sources[i].clip.name)
            {
                _sources[i].Play();
            }
        }
    }
}
