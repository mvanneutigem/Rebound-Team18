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

    //for quick sounds that may layer over the top of each other
    public void PlaySFX(string name)
    {
        for (int i = 0; i < _sources.Length; i++)
        {
            if (_sources[i].clip)
            {
                if (name == _sources[i].clip.name)
                {
                    _sources[i].PlayOneShot(_sources[i].clip);
                }
            }
        }
    }

    //for longer sounds, like music or ambient noise
    public void PlayLong(string name)
    {
        for (int i = 0; i < _sources.Length; i++)
        {
            if (_sources[i].clip)
            {
                if (name == _sources[i].clip.name)
                {
                    _sources[i].Play();
                }
            }
        }
    }
}
