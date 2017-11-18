using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public GameObject[] enemyPlayer; 
	public Vector3 spawnValues; 
	private float spawnWait; 
	public float spawnMostWait;
	public float spawnLeastWait; 
	public int startWait; 
	public bool stop; 
	private Quaternion originalRot = Quaternion.Euler(0, 0, 0);
	private Transform myTransform;


	void Start () 
	{
		myTransform = this.transform;
		StartCoroutine (waitSpawner()); 
	}

	void Update () 
	{
		spawnWait = Random.Range (spawnLeastWait, spawnMostWait); 
	}

	IEnumerator waitSpawner()
	{
		yield return new WaitForSeconds (startWait); 

		while (!stop) 
		{
			Vector2 spawnPosition = new Vector2(myTransform.position.x, myTransform.position.y);
			Debug.Log ("x " + myTransform.position.x + " y " + myTransform.position.y); 

			Instantiate (enemyPlayer[0], spawnPosition, originalRot); 

			yield return new WaitForSeconds (spawnWait); 
		}
	}
}