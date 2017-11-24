using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
    public GameObject player;
    public float camZpos = -10f;
    public float camXoff = 5f;
    public float camYoff = 1f;

    public float horzSpeed = 2f;
    public float vertSpeed = 2f;

    private Transform _camera;
    private playerController _playerController;


	// Use this for initialization
	void Start ()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        _playerController = player.GetComponent<playerController>();
        _camera = Camera.main.transform;
        _camera.position = new Vector3(
            player.transform.position.x + camXoff, 
            player.transform.position.y + camYoff, 
            player.transform.position.z + camZpos);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_playerController.isFacingRight)
        {
            //this moves the camera over time and not just immediately
            // _camera.position = new Vector3(Mathf.Lerp(_camera.position.x, player.transform.position.x + camXoff, horzSpeed * Time.deltaTime),
            //  Mathf.Lerp(_camera.position.y, player.transform.position.y + camYoff, vertSpeed * Time.deltaTime), camZpos);
            _camera.position = new Vector3(player.transform.position.x + camXoff, player.transform.position.y + camYoff, camZpos);
        }
        else
        {
            //this moves the camera over time and not just immediately
           // _camera.position = new Vector3(Mathf.Lerp(_camera.position.x, player.transform.position.x - camXoff, horzSpeed * Time.deltaTime),
               // Mathf.Lerp(_camera.position.y, player.transform.position.y - camYoff, vertSpeed * Time.deltaTime), camZpos);
            _camera.position = new Vector3(player.transform.position.x, player.transform.position.y + camYoff, camZpos);//
        }
	}
}
