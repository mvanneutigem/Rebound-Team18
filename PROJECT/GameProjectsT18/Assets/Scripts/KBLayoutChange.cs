using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KBLayoutChange : MonoBehaviour {

	// Use this for initialization
    public Dropdown _dd;

    void Start()
    {
        _dd.value =  1 - PlayerPrefs.GetInt("qwerty", 1);
    }

    public void SetLayout()
    {
        PlayerPrefs.SetInt("qwerty", 1 - _dd.value);
    }
}
aaa