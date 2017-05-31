using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    private AudioManager _audioManager;
    public GameObject _coll;
    public float power = 1000;
    private string slam;
    private bool _activated = false;
    // Use this for initialization
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsPlayerController>().SetLockMovement(true);
        slam = PlayerPrefs.GetString("Slam");
        _activated = false;
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();


    }

    void Update()
    {
        if ((Input.GetButtonUp("Slam") || Input.GetKeyUp(slam)) && !_activated)
        {
            Launch();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().StartGame();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rewind>().enabled = true;
            _activated = true;
        }

    }
    private void Launch()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsPlayerController>().SetLockMovement(false);
        int Id = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(Id);
        if (Id < 10)
        {
            _audioManager.PlayLong("startupshort");
        }
        else if (Id > 9 && Id < 15)
        {
            _audioManager.PlayLong("dillon");
        }
        else if (Id > 14 && Id < 25)
        {
            _audioManager.PlayLong("neonstorm");
        }
        _audioManager.PlaySFX("spring");
        StartCoroutine(playanimation());


    }

    private IEnumerator playanimation()
    {
        var pos = _coll.transform.position;
        var rot = _coll.transform.rotation;
        GetComponent<Animation>()["launch"].speed = 1;
        GetComponent<Animation>()["launch"].time = 0;
        GetComponent<Animation>().Play();
        _coll.GetComponent<Rigidbody>().AddForce(0, 0, power);
        yield return new WaitForSeconds(0.1f);
        _coll.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _coll.transform.position = pos;
        _coll.transform.rotation = rot;
    }
}
