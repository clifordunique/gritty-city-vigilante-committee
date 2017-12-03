using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public GameObject[] enemyPlayer; 
	private Vector3 spawnValues; 
	private float spawnWait; 
	public float spawnMostWait;
	public float spawnLeastWait; 
	public int startWait; 
	public bool stop; 
	private Quaternion originalRot = Quaternion.Euler(0, 0, 0);
	private Transform myTransform;
	private int enemiesKilled = 2; 

	void Start () 
	{
		myTransform = this.transform;
		StartCoroutine (waitSpawner()); 
	}


	void Update () 
	{
		spawnWait = Random.Range (spawnLeastWait, spawnMostWait); 
	}

	public void enemyKilled ()
	{
		enemiesKilled++;
	}

	IEnumerator waitSpawner()
	{
		yield return new WaitForSeconds (startWait); 

		while (!stop) 
		{
			yield return new WaitForSeconds (spawnWait);
			Vector2 spawnPosition = new Vector2(myTransform.position.x, myTransform.position.y);
			Debug.Log ("x " + myTransform.position.x + " y " + myTransform.position.y); 
			if (enemiesKilled >= 2) 
			{
				enemiesKilled = 0; 
				Instantiate (enemyPlayer[0], spawnPosition, originalRot); 
				yield return new WaitForSeconds (2);
				Instantiate (enemyPlayer[1], spawnPosition, originalRot); 

			}
		}
	}
}