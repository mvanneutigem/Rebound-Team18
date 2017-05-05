using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashScript : MonoBehaviour {

    public void Squash()
    {
        var scale = transform.localScale;
        scale.y = 0.5f;
        scale.x = 1.5f;
        scale.z = 1.5f;
        transform.localScale = scale;
    }

    public void Stretch()
    {
        var scale = transform.localScale;
        scale.y = 1.5f;
        scale.x = 0.5f;
        scale.z = 0.5f;
        transform.localScale = scale;
    }

    public void NoSquash()
    {
        var scale = transform.localScale;
        scale.y = 1.0f;
        scale.y = 1.0f;
        scale.y = 1.0f;
        transform.localScale = scale;
    }
}
