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

	private bool bossFight = false; 
	private bool specialMove = false; 
	private bool bossDied = false;


	public float defaultSize = 21.28404f; 
	public float bossFightSize = 40f; 
	public float specialMoveSize = 30f; 


	private GameObject bossFightCameraPosition; 
	public Vector2 bossCameraPosition = new Vector2(40,0); 

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

		bossFightCameraPosition = GameObject.FindGameObjectWithTag("bossFightCameraPosition");
	}
	
	// Update is called once per frame
	void Update ()
    {
		checkLocation (); 
		if (bossFight) {
			bossBehavior ();
		} else if (specialMove) {
			specialBehavior (); 
		} else {
			normalBehavior (); 
		}
        
	}

	private void checkLocation ()
	{
		if (player.transform.position.x > bossFightCameraPosition.transform.position.x && bossDied == false) 
		{
			bossFight = true; 
			// Debug.Log ("boss fight set"); 
		}
	}

	private void specialBehavior() {
		this.GetComponent<Camera> ().orthographicSize = specialMoveSize;
	}

	private void bossBehavior()
	{
		this.GetComponent<Camera> ().orthographicSize = bossFightSize;
	}

	private void normalBehavior()
	{
		this.GetComponent<Camera> ().orthographicSize = defaultSize;
		if (_playerController.isFacingRight) {
			//this moves the camera over time and not just immediately
			// _camera.position = new Vector3(Mathf.Lerp(_camera.position.x, player.transform.position.x + camXoff, horzSpeed * Time.deltaTime),
			//  Mathf.Lerp(_camera.position.y, player.transform.position.y + camYoff, vertSpeed * Time.deltaTime), camZpos);
			_camera.position = new Vector3 (player.transform.position.x + camXoff, player.transform.position.y + camYoff, camZpos);
		} else {
			//this moves the camera over time and not just immediately
			// _camera.position = new Vector3(Mathf.Lerp(_camera.position.x, player.transform.position.x - camXoff, horzSpeed * Time.deltaTime),
			// Mathf.Lerp(_camera.position.y, player.transform.position.y - camYoff, vertSpeed * Time.deltaTime), camZpos);
			_camera.position = new Vector3 (player.transform.position.x, player.transform.position.y + camYoff, camZpos);//
		}
	}

	public void setBossDied()
	{
		Debug.Log ("Boss Died Called"); 
		bossFight = false;
		bossDied = true;
	}
}
